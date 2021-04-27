using System;
using System.Collections.Generic;
using Cinary.Finance.Qif;
using Cinary.Finance.Qif.Transaction;

namespace Jar.Import
{
	public class TransactionEntry : ITransactionEntry
	{
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public decimal Amount { get; set; }
		public IList<decimal> SplitAmount { get; set; }
		public IList<decimal> SplitAmounts { get; set; }
		public bool IsCleared { get; set; }
		public string Num { get; set; }
		public IList<string> PayeeLines { get; set; }
		public string Memo { get; set; }
		public IList<string> SplitMemo { get; set; }
		public IList<string> AddressLines { get; set; }
		public string Category { get; set; }
		public IList<string> SplitCategory { get; set; }
		public string Type { get; set; }

		public string Address { get; set; }

		public string Payee { get; set; }

		public string PayeeAccount { get; set; }

		public string PayeeName { get; set; }
	}

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

		public void Import(DataModel Model, string Filename, int Account, int Currency)
		{
			var reader = new QifReader();
			var transactions = reader.ReadFromFile<TransactionEntry>(Filename);

			Model.Connection.BeginTransaction();

			foreach(var inputTransaction in transactions)
			{
				var outputTransaction = new Model.Transaction();
				outputTransaction.Account = Account;
				outputTransaction.Date = inputTransaction.Date;
				outputTransaction.Payee = inputTransaction.Payee;
				outputTransaction.Memo = inputTransaction.Memo;
				outputTransaction.Amount = (int)Math.Round((decimal)100.0 * inputTransaction.Amount);

				Model.Connection.Insert(outputTransaction);
			}

			Model.Connection.Commit();
		}
	}
}
