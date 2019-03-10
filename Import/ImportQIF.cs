using QifApi;
using System;
using System.Collections.Generic;
using System.IO;

namespace Jar.Import
{
	public class ImportQIF : IImport
	{
		public string[] Extensions()
		{
			return new string[] { ".qif" };
		}

		public string FormatName()
		{
			return "Quicken";
		}

		void ProcessCategory( IEnumerable<QifApi.Transactions.BasicTransaction> Transactions, DataModel Model, int Account, int Currency)
		{
			foreach (var inputTransaction in Transactions)
			{
				var outputTransaction = new Model.Transaction();
				outputTransaction.Account = Account;
				outputTransaction.Date = inputTransaction.Date;
				outputTransaction.Payee = inputTransaction.Payee;
				outputTransaction.Memo = inputTransaction.Memo;
				outputTransaction.Amount = (int)Math.Round((decimal)100.0 * inputTransaction.Amount);

				Model.Connection.Insert(outputTransaction);
			}
		}

		public void Import(DataModel Model, string Filename, int Account, int Currency)
		{
			QifDom qifDom = QifDom.ImportFile(Filename);

			Model.Connection.BeginTransaction();
			ProcessCategory(qifDom.BankTransactions, Model, Account, Currency);
			ProcessCategory(qifDom.AssetTransactions, Model, Account, Currency);
			ProcessCategory(qifDom.CashTransactions, Model, Account, Currency);
			ProcessCategory(qifDom.CreditCardTransactions, Model, Account, Currency);
			ProcessCategory(qifDom.LiabilityTransactions, Model, Account, Currency);
			Model.Connection.Commit();
		}
	}
}
