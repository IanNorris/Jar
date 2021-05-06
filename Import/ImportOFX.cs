using System;

namespace Jar.Import
{
	public class ImportOFX : IImport
	{
		public string[] Extensions()
		{
			return new string[] { ".ofx" };
		}

		public string FormatName()
		{
			return "Microsoft Money";
		}

		public void Import(DataModel Model, string Filename, int Account, int Currency, int BatchId )
		{
			var parser = new OFXParser.OFXParser();
			var ofxDocument = parser.GenerateExtract(Filename);

			Model.Connection.BeginTransaction();
			foreach ( var inputTransaction in ofxDocument.Transactions )
			{
				var outputTransaction = new Model.Transaction();
				outputTransaction.ImportBatch = BatchId;
				outputTransaction.Currency = Currency;
				outputTransaction.Account = Account;
				outputTransaction.Date = inputTransaction.Date;
				outputTransaction.Payee = inputTransaction.Description;
				outputTransaction.Amount = (long)Math.Round(100.0 * inputTransaction.TransactionValue);

				Model.Connection.Insert(outputTransaction);
			}
			Model.Connection.Commit();
		}
	}
}
