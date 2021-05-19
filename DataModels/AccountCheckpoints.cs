using System;
using SQLite;

namespace Jar.DataModels
{
	public class AccountCheckpoints
	{
		public AccountCheckpoints(EventBus eventBus)
		{
		}

		public void SetDatabase(Database database)
		{
			_database = database;
		}

		private Database _database;
	}
}
