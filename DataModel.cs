using SQLite;
using Jar.Model;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Jar
{
	public class DataModel
	{
		private SQLiteConnection m_database;

		public SQLiteConnection Connection { get { return m_database; } }

		public static SQLiteConnection CreateDatabase(string Path, string Password)
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

		public DataModel(string Path, string Password)
		{
			m_database = CreateDatabase(Path, Password);
		}

		public IEnumerable<Transaction> GetTransactions()
		{
			return m_database.Table<Transaction>();
		}
	}
}
