using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jar.Model;
using JarPluginApi;

namespace Jar.Import
{
	public class ImportOFX : IImport
	{
		public ImportType Type()
		{
			return ImportType.File;
		}

		public string[] Extensions()
		{
			return new string[] { ".ofx" };
		}

		public string FormatName()
		{
			return "Microsoft Money";
		}

		public Task<List<Transaction>> Import(string AccountName, string Filename, int Account, int Currency, int BatchId, DateTime ImportFrom)
		{
			var parser = new OFXParser.OFXParser();
			var ofxDocument = parser.GenerateExtract(Filename);

			var outputList = new List<Transaction>();
			foreach (var inputTransaction in ofxDocument.Transactions)
			{
				var outputTransaction = new Transaction();
				outputTransaction.ImportBatchId = BatchId;
				outputTransaction.Currency = Currency;
				outputTransaction.AccountId = Account;
				outputTransaction.Date = inputTransaction.Date;
				outputTransaction.Payee = inputTransaction.Description;
				outputTransaction.Amount = (long)Math.Round(100.0 * inputTransaction.TransactionValue);

				outputList.Add(outputTransaction);
			}

			return Task.FromResult(outputList);
		}
	}
}
