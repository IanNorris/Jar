using JarPluginApi;

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

		public async Task<List<Transaction>> Import(string AccountName, string Filename, int Account, int Currency, int BatchId)
		{
			var pat = _configService.GetConfigValue("PAT");

			_client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", pat);

			var result = await _client.GetAsync($"{ServiceEndpoint}{AccountsEndpoint}");

			var resultBody = await result.Content.ReadAsStringAsync();

			if ( result.StatusCode != System.Net.HttpStatusCode.OK )
			{
				throw new HttpRequestException($"Got {result.StatusCode} {resultBody}");
			}

			const string BalanceEndpoint = $"/api/v2/account-holder/joint";

			var result2 = await _client.GetAsync($"{ServiceEndpoint}{BalanceEndpoint}");

			var resultBody2 = await result2.Content.ReadAsStringAsync();

			if (result2.StatusCode != System.Net.HttpStatusCode.OK)
			{
				throw new HttpRequestException($"Got {result2.StatusCode} {resultBody2}");
			}

			return new List<Transaction>();
		}

		private IConfigService _configService;

		private HttpClient _client = new HttpClient();

		private const string ServiceEndpoint = "https://api.starlingbank.com";
		private const string SandboxServiceEndpoint = "https://api-sandbox.starlingbank.com";

		private const string AccountsEndpoint = "/api/v2/accounts";
	}
}
