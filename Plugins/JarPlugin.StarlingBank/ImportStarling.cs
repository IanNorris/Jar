using JarPlugin.StarlingBank.Models;
using JarPluginApi;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;

namespace JarPlugin.StarlingBank.Import
{
	class ImportStarling : IImport
	{
		public ImportStarling(IConfigService configService)
		{
			_configService = configService;
		}

		public ImportType Type()
		{
			return ImportType.Online;
		}

		public string[] Extensions()
		{
			throw new NotImplementedException();
		}

		public string FormatName()
		{
			return "Starling Bank";
		}

		public void ThrowOnError<T>(RestResult<T> result, string message) where T : StarlingError
		{
			if (!(result.IsSuccess && result.Result.Success))
			{
				var errors = string.Join("\r\n", result.Result.Errors);
				throw new InvalidDataException($"{message}.\nStatus code {result.StatusCode.ToString("G")} {result.StatusCode.ToString("D")}\n{errors}");
			}
		}

		public void ThrowOnError(RestResult<string> result, string message)
		{
			if (!result.IsSuccess)
			{
				//We did something wrong
				if ((int)result.StatusCode >= 400 && (int)result.StatusCode < 500)
				{
					var errorPayload = JsonConvert.DeserializeObject<StarlingError>(result.Result, RestClient.GetSerializationPolicy(camelCaseJson: true));

					if (errorPayload != null && errorPayload.Errors != null)
					{
						var errors = string.Join("\r\n", errorPayload.Errors);
						throw new InvalidDataException($"{message}.\nStatus code {result.StatusCode.ToString("G")} {result.StatusCode.ToString("D")}\n{errors}");
					}
				}

				throw new InvalidDataException($"{message}.\nStatus code {result.StatusCode.ToString("G")} {result.StatusCode.ToString("D")}");
			}
		}

		public List<Transaction> ReadCsv(string csvBody, int Account, int Currency, int BatchId)
		{
			var outputList = new List<Transaction>();

			using var memoryStream = new MemoryStream();
			using var streamWriter = new StreamWriter(memoryStream);

			streamWriter.Write(csvBody);
			streamWriter.Flush();

			memoryStream.Position = 0;

			using var parser = new TextFieldParser(memoryStream);

			parser.TextFieldType = FieldType.Delimited;
			parser.SetDelimiters(",");
			bool firstRow = true;
			while (!parser.EndOfData)
			{
				string[] fields = parser.ReadFields();

				if (firstRow)
				{
					firstRow = false;
					continue;
				}

				var date = fields[0];
				var counterParty = fields[1];
				var reference = fields[2];
				var type = fields[3];
				var amount = fields[4];
				var balance = fields[5];
				var spendingCategory = fields[6];
				var notes = fields[7];

				var outputTransaction = new Transaction();
				outputTransaction.ImportBatchId = BatchId;
				outputTransaction.Currency = Currency;
				outputTransaction.AccountId = Account;
				outputTransaction.Date = DateTime.Parse(date);
				outputTransaction.Payee = counterParty;
				outputTransaction.Reference = reference;
				outputTransaction.Amount = (long)Math.Round(100 * decimal.Parse(amount));
				outputTransaction.ReferenceBalanceFromImport = (long)Math.Round(100 * decimal.Parse(balance));
				outputTransaction.ImportedCategory = spendingCategory;
				outputTransaction.Memo = notes;

				outputList.Add(outputTransaction);
			}

			return outputList;
		}

		public async Task<List<Transaction>> Import(string AccountName, string Filename, int Account, int Currency, int BatchId, DateTime ImportFrom)
		{
			var pat = _configService.GetConfigValue(AccountName, "PAT");
			var devModeString = _configService.GetConfigValue(AccountName, "DevAccount");
			bool.TryParse(devModeString, out var devMode);
			var endpoint = devMode ? SandboxServiceEndpoint : ServiceEndpoint;

			using var client = new RestClient(endpoint, pat, deserializeOnError: true, true);

			var result = await client.GetObjectAsync<StarlingAccounts>(AccountsEndpoint);

			ThrowOnError(result, "Failed to get account list");

			var accountUid = result?.Result?.Accounts.FirstOrDefault()?.AccountUid;
			if (accountUid == null)
			{
				throw new InvalidDataException("accountUid is null");
			}

			var csvFile = await client.GetStringAsync("text/csv", DownloadStatementEndpoint, accountUid, ImportFrom.ToString("yyyy-MM-dd"));

			ThrowOnError(csvFile, "Failed to get transactions");

			return ReadCsv(csvFile.Result, Account, Currency, BatchId);
		}

		private IConfigService _configService;

		private HttpClient _client = new HttpClient();

		private const string ServiceEndpoint = "https://api.starlingbank.com";
		private const string SandboxServiceEndpoint = "https://api-sandbox.starlingbank.com";

		private const string AccountsEndpoint = "/api/v2/accounts";
		private const string DownloadStatementEndpoint = "/api/v2/accounts/{0}/statement/downloadForDateRange?start={1}";
	}
}
