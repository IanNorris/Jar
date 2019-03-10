using SQLite;
using Jar.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using Jar.Import;

namespace Jar
{
	public class DataModel
	{
		private SQLiteConnection m_database;
		private Importer m_import;

		public SQLiteConnection Connection { get { return m_database; } }
		public Importer Import { get { return m_import; } }

		public SQLiteConnection CreateDatabase(string Path, string Password)
		{
			var ConnectionString = new SQLiteConnectionString(
				Path,
				true,
				key: Password
			);

			m_database = new SQLiteConnection(ConnectionString);
			m_database.CreateTable<Transaction>();
			m_database.CreateTable<Account>();

			/*m_database.BeginTransaction();

			m_database.Insert(new Transaction()
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

			m_database.Commit();*/

			m_import = new Importer(this);

			//var FileToImport = @"FileToImport.qif";
			//m_import.Import(FileToImport, 123, 5);
			
			return m_database;
		}

		public DataModel(string Path, string Password)
		{
			m_database = CreateDatabase(Path, Password);
		}

		public IEnumerable<Account> GetAccounts()
		{
			return m_database.Table<Account>();
		}

		public IEnumerable<Transaction> GetTransactions()
		{
			return m_database.Table<Transaction>();
		}

		public IEnumerable<Transaction> GetTransactions(System.Linq.Expressions.Expression<Func<Transaction,bool>> Predicate)
		{
			return m_database.Table<Transaction>().Where(Predicate);
		}

		public IEnumerable<Transaction> GetTransactionsBetweenDates( DateTime Start, DateTime End )
		{
			return GetTransactions(t => t.Date >= Start && t.Date < End);
		}
	}
}
