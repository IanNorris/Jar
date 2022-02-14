using System;
using System.Collections.Generic;
using System.Linq;
using Jar.Model;
using JarPluginApi;

namespace Jar.DataModels
{
	public class Accounts
	{
		public Accounts(EventBus eventBus)
		{
		}

		public void SetDatabase(Database database)
		{
			_database = database;
		}

		public IEnumerable<Account> GetAccounts()
		{
			var results = _database.Connection.Table<Account>().ToArray();
			foreach (var result in results)
			{
				result.LastBalance = _database.Connection.Table<Transaction>().Where(t => t.AccountId == result.Id).Select(t => t.Amount).Sum();
			}

			return results;
		}

		public int CreateAccount(string name, AccountType type, int currency)
		{
			var lastAccountOrder = _database.Connection.Table<Account>().Select(a => a.Order).DefaultIfEmpty(0).Max();

			var newAccount = new Account
			{
				Currency = currency,
				IsOpen = true,
				LastBalance = 0,
				LastSettled = DateTime.UtcNow,
				Name = name,
				Order = lastAccountOrder + 1,
				Type = type,
			};

			_database.Connection.Insert(newAccount);

			var accountId = (int)_database.GetLastInsertedRowId();

			return accountId;
		}

		private Database _database;
	}
}
