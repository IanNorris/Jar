using System;

using SQLite;

namespace Jar.Model
{
	public class Jar
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public int CategoryId { get; set; }
		public string Name { get; set; }
		public JarType Type { get; set; }
		public string Filters { get; set; }
		public long MonthlyValue { get; set; }
		public long TargetValue { get; set; }
		public DateTime TargetDate { get; set; }
		public bool CarryOver { get; set; }
		public bool FlagTransactionCount { get; set; }
		public bool FlagTotalAmount { get; set; }

		public int Order { get; set; }

		//
		//NOTE: Adding new fields defaults to NULL
		//
	}
}
