﻿using System;
using SQLite;
using Jar.Model;
using static Jar.MessageBox;

namespace Jar
{
	public class Database
	{
		public Database(ShowMessageDelegate showMessage)
		{
			_showMessage = showMessage;
		}

		public bool CreateDatabase(string Filename, string Password)
		{
			var ConnectionString = new SQLiteConnectionString( Filename, true, key: Password );

			try
			{
				_database = new SQLiteConnection(ConnectionString);

				PrepareDatabase();

				return true;
			}
			catch (Exception)
			{
				if (_showMessage != null)
				{
					_showMessage("Invalid password, your budget is corrupted or you selected a file that is not a budget.", "Unable to open budget", MessageIcon.Error, false, false);
				}

				_database = null;
				return false;
			}
		}

		private void PrepareDatabase()
		{
			_database.CreateTable<Account>(CreateFlags.ImplicitIndex);
			_database.CreateTable<Transaction>(CreateFlags.ImplicitIndex);
			_database.CreateTable<ImportBatch>(CreateFlags.ImplicitIndex);
		}

		public long GetLastInsertedRowId()
		{
			return SQLite3.LastInsertRowid(_database.Handle);
		}

		public SQLiteConnection Connection => _database;

		private SQLiteConnection _database;
		private ShowMessageDelegate _showMessage;
	}
}