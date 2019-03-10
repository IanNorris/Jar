using OFXSharp;
using System;
using System.IO;

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
			var parser = new OFXDocumentParser();
			var ofxDocument = parser.Import(new FileStream(Filename, FileMode.Open));

			//TODO
			//ofxDocument.Currency
			//ofxDocument.Balance.AvaliableBalance
			//ofxDocument.Balance.AvaliableBalanceDate

			//TODO: Apply Currency rules.

			Model.Connection.BeginTransaction();
			foreach ( var inputTransaction in ofxDocument.Transactions )
			{
				//TODO
				//inputTransaction.Currency

				var outputTransaction = new Model.Transaction();
				outputTransaction.Account = Account;
				outputTransaction.Date = inputTransaction.Date;
				outputTransaction.Payee = inputTransaction.Name;
				outputTransaction.Memo = inputTransaction.Memo;
				outputTransaction.Amount = (int)Math.Round((decimal)100.0 * inputTransaction.Amount);

				Model.Connection.Insert(outputTransaction);
			}
			Model.Connection.Commit();
		}
	}
}
