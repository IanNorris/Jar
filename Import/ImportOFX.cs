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

		public void Import(DataModel Model, string Filename, int Account, int Currency )
		{
			var parser = new OFXParser.OFXParser();
			var ofxDocument = parser.GenerateExtract(Filename);

			Model.Connection.BeginTransaction();
			foreach ( var inputTransaction in ofxDocument.Transactions )
			{
				//TODO
				//inputTransaction.Currency

				var outputTransaction = new Model.Transaction();
				outputTransaction.Account = Account;
				outputTransaction.Date = inputTransaction.Date;
				outputTransaction.Payee = inputTransaction.Description;
				outputTransaction.Amount = (int)Math.Round(100.0 * inputTransaction.TransactionValue);

				Model.Connection.Insert(outputTransaction);
			}
			Model.Connection.Commit();
		}
	}
}
