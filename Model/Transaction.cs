using SQLite;
using System;

namespace Jar.Model
{
	public class Transaction
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public int Account { get; set; }

		public DateTime Date { get; set; }
		public string Payee { get; set; }
		public string OriginalPayee { get; set; }
		public string Memo { get; set; }
		public string Reference { get; set; }
		public int Category { get; set; }
		public string ImportedCategory { get; set; }
		public int Currency { get; set; }
		public int ConversionRate { get; set; }
		public int Amount { get; set; }

		public int IsAccepted { get; set; }
		public int Flag { get; set; }

		public int ImportBatch { get; set; }
		public bool ManualOrEdited { get; set; }
	}
}
