using System;
using SQLite;

namespace Jar.DataModels
{
	public class JarEntry
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public int JarId { get; set; }

		public DateTime BudgetDate { get; set; }

		public long MonthlyAssignedValue { get; set; }
		public long TotalValue { get; set; }
		public long CarriedOverValue { get; set; }
	}
}
