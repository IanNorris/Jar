using SQLite;
using System;

namespace Jar.Model
{
	public enum AccountType
	{
		Current,
		Credit,
		Savings,
		Mortgage,
		Investment,
		Pension
	}

	public class Account
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public string Name { get; set; }

		public AccountType Type { get; set; }

		public DateTime LastSettled { get; set; }

		public bool IsOpen { get; set; }
		public int Order { get; set; }
		public int Currency { get; set; }

		public long LastBalance { get; set; }

		//
		//NOTE: Adding new fields defaults to NULL
		//
	}
}
