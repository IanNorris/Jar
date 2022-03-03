using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using JarPluginApi;

namespace Jar.Import
{
	public class ImportQIF : IImport
	{
		public ImportType Type()
		{
			return ImportType.File;
		}

		public string[] Extensions()
		{
			return new string[] { ".qif" };
		}

		public string FormatName()
		{
			return "Quicken";
		}

		public async Task<List<Transaction>> Import(string AccountName, string Filename, int Account, int Currency, int BatchId)
		{
			var outputList = new List<Transaction>();

			var lines = await File.ReadAllLinesAsync(Filename);

			var outputTransaction = new Transaction();

			foreach (var line in lines)
			{
				if (line.Length == 0)
				{
					continue;
				}

				switch (line[0])
				{
					case '!': //Header
					case 'C': //Cleared status
					case 'N': //Check number
					case 'A': //Address
						continue;

					case '^':
						{
							outputTransaction.Currency = Currency;
							outputTransaction.AccountId = Account;
							outputTransaction.ImportBatchId = BatchId;
							outputList.Add(outputTransaction);
							outputTransaction = new Transaction();
							break;
						}

					case 'D':
						{
							outputTransaction.Date = DateTime.Parse(line.Substring(1));
							break;
						}

					case 'T':
						{
							outputTransaction.Amount = (long)Math.Round(100 * decimal.Parse(line.Substring(1)));
							break;
						}

					case 'U':
						{
							outputTransaction.Amount = (long)Math.Round(100 * decimal.Parse(line.Substring(1)));
							break;
						}

					case 'P':
						{
							outputTransaction.Payee = line.Substring(1);
							break;
						}

					case 'M':
						{
							outputTransaction.Memo = line.Substring(1);
							break;
						}

					case 'L':
						{
							outputTransaction.ImportedCategory = line.Substring(1);
							break;
						}

				}
			}

			return outputList;
		}
	}
}
