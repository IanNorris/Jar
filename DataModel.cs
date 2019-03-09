using SQLite;
using Jar.Model;
using System;

namespace Jar
{
	static class DataModel
	{
		public static SQLiteConnection Create(string Path, string Password)
		{
			var ConnectionString = new SQLiteConnectionString(
				Path,
				true,
				key: Password
			);

			var DB = new SQLiteConnection(ConnectionString);
			DB.CreateTable<Transaction>();

			DB.BeginTransaction();

			DB.Insert(new Transaction()
			{
				Date = DateTime.UtcNow,
				Payee = "Arbees",
				Memo = "Dinner out",
				Note = "Dinner with the wife",
				Category = 123,
				Currency = 3,
				ConversionRate = 0,
				Amount = 5300
			});

			DB.Commit();

			return DB;
		}

	}
}
