using SQLite;

namespace JarPluginApi
{
	public class Transaction
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public int LiveTransactionId { get; set; } // If deleted, this refers to an undeleted transaction that the history is tied to

		public int AccountId { get; set; }

		public DateTime Date { get; set; }
		public DateTime EditDate { get; set; } = DateTime.UtcNow;
		public string? Payee { get; set; }
		public string? OriginalPayee { get; set; }
		public string? Memo { get; set; }
		public string? Reference { get; set; }
		public int JarId { get; set; }
		public int? CheckpointId { get; set; }
		public string? ImportedCategory { get; set; }
		public int Currency { get; set; }
		public int ConversionRate { get; set; }
		public long Amount { get; set; }
		public long? ReferenceBalanceFromImport { get; set; }

		public int IsAccepted { get; set; }
		public int Flag { get; set; }

		public int ImportBatchId { get; set; }
		public bool ManualOrEdited { get; set; }
		public bool Deleted { get; set; } = false;
		public bool NeedsReview { get; set; } = false;

		//
		//NOTE: Adding new fields defaults to NULL
		//
	}
}
