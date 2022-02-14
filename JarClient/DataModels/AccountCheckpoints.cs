using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Jar.Model;
using JarPluginApi;

namespace Jar.DataModels
{
	public class AccountCheckpoints
	{
		public AccountCheckpoints(Transactions transactions, EventBus eventBus)
		{
			_transactions = transactions;
		}

		public void SetDatabase(Database database)
		{
			_database = database;
		}

		public AccountCheckpoint GetCheckpoint(DateTime startDate)
		{
			return _database.Connection.Table<AccountCheckpoint>().Where(c => c.StartDate == startDate).Take(1).FirstOrDefault();
		}

		public void UpdateAccountCheckpoints(int account)
		{
			var firstTransaction = _transactions.GetFirstTransactionAfter(account, DateTime.MinValue);
			var lastTransaction = _transactions.GetLastTransaction(account);

			if (firstTransaction == null || lastTransaction == null)
			{
				return;
			}

			if (firstTransaction.Id == lastTransaction.Id)
			{
				return;
			}

			var previousBalance = 0L;
			AccountCheckpoint lastCheckpoint = null;
			AccountCheckpoint newCheckpoint = null;

			var transactionsOutsideCheckpoints = _database.Connection.QueryScalars<int>(@"SELECT COUNT() FROM 
	(SELECT [Transaction].*, [AccountCheckpoint].Id AS LinkedCheckpointId, StartDate, EndDate 
		FROM [Transaction] 
		INNER JOIN AccountCheckpoint ON [Transaction].CheckpointId = [AccountCheckpoint].Id)
WHERE AccountId = ? AND NOT (Date >= StartDate AND Date < EndDate AND CheckpointId = LinkedCheckpointId)", new object[] { account }).First();

			var transactionsWithoutCheckpoints = _database.Connection.QueryScalars<int>(@"SELECT COUNT() FROM [Transaction] WHERE CheckpointId IS NULL AND AccountId = ?", new object[] { account }).First();

			if (transactionsWithoutCheckpoints == 0 && transactionsOutsideCheckpoints == 0)
			{
				return;
			}

			//Need to rebuild.

			_database.Connection.Table<AccountCheckpoint>().Delete(c => c.AccountId == account);

			var existingCheckpoints = _database.Connection.Table<AccountCheckpoint>().Where(c => c.AccountId == account).Count();
			if (existingCheckpoints != 0)
			{
				throw new InvalidDataException($"No checkpoints should exist for account {account}.");
			}

			var startPeriod = GetStartOfMonth(firstTransaction.Date);
			var endPeriod = GetEndOfMonth(lastTransaction.Date);

			var transactionsToUpdate = new List<Transaction>();

			while (startPeriod < endPeriod)
			{
				var nextPeriod = startPeriod.AddMonths(1);

				var transactionsInRange = _transactions.GetTransactionsBetweenDates(startPeriod, nextPeriod, account).ToList();

				var transactionTotal = transactionsInRange.Sum(t => t.Amount);

				newCheckpoint = new AccountCheckpoint()
				{
					StartDate = startPeriod,
					EndDate = nextPeriod,
					StartBalance = previousBalance,
					EndBalance = previousBalance + transactionTotal,
					AccountId = account,
					PreviousCheckpointId = lastCheckpoint?.Id
				};

				_database.Connection.Insert(newCheckpoint);

				foreach (var transaction in transactionsInRange)
				{
					transaction.CheckpointId = newCheckpoint.Id;
				}

				transactionsToUpdate.AddRange(transactionsInRange);

				startPeriod = nextPeriod;
				lastCheckpoint = newCheckpoint;
				previousBalance = newCheckpoint.EndBalance;
			}

			_database.Connection.UpdateAll(transactionsToUpdate, true);
		}

		private DateTime GetStartOfMonth(DateTime date)
		{
			return new DateTime(date.Year, date.Month, 1);
		}

		private DateTime GetEndOfMonth(DateTime date)
		{
			return new DateTime(date.Date.Year, date.Date.Month, 1).AddMonths(1);
		}

		private Database _database;
		private Transactions _transactions;
	}
}
