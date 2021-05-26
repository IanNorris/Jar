using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Jar.Import;
using Jar.Model;

namespace Jar.DataModels
{
	public class Transactions
	{
		public Transactions(EventBus eventBus)
		{
			_import = new Importer();
		}

		public void SetDatabase(Database database)
		{
			_database = database;
		}

		public IEnumerable<DisplayTransaction> GetDisplayTransactions(DateTime start, DateTime end, int account)
		{
			long runningTotal = 0;
			int? currentCheckpointId = null;
			long previousTotal = long.MinValue;

			var preTransactions = GetTransactionsBetweenDates(new DateTime(start.Year, start.Month, 1), start, account).OrderBy(t => t.Date);

			foreach(var transaction in preTransactions)
			{
				if (transaction.CheckpointId != currentCheckpointId)
				{
					runningTotal = 0;
					currentCheckpointId = transaction.CheckpointId;
				}

				runningTotal += transaction.Amount;
			}

			//We borrow the Balance field from the transaction temporarily
			//as the StartBalance from the account checkpoint
			var results = _database.Connection.Query<DisplayTransaction>(@"SELECT [Transaction].*, StartBalance AS Balance FROM [Transaction] INNER JOIN [AccountCheckpoint] On [Transaction].CheckpointId = [AccountCheckpoint].Id WHERE Date >= ? AND Date < ? AND [Transaction].AccountId = ? ORDER BY Date ASC", new object[] { start, end, account });

			foreach(var result in results)
			{
				if(result.CheckpointId != currentCheckpointId)
				{
					if(result.Balance != previousTotal && previousTotal != long.MinValue)
					{
						throw new InvalidDataException($"Balance pre transaction for #{result.Id} of {result.Balance} does not match previous sum of {previousTotal}.");
					}

					runningTotal = 0;
					currentCheckpointId = result.CheckpointId;
				}

				runningTotal += result.Amount;

				var newBalance = runningTotal + result.Balance;
				result.Balance = newBalance;

				previousTotal = newBalance;
			}

			return results;

			/*long runningTotal = 0;
			return transactions.Join(_database.Connection.Table<AccountCheckpoint>().AsQueryable(), t => t.Id, c => c.Id, (t, c) =>
			{
				runningTotal += t.Amount;
				return new DisplayTransaction() { Transaction = PrepareDisplayTransaction(t), Balance = (c?.StartBalance ?? 0) + runningTotal };
			});*/
		}

		public IEnumerable<Transaction> GetTransactions(Expression<Func<Transaction, bool>> predicate)
		{
			return _database.Connection.Table<Transaction>().Where(predicate).OrderBy( t => t.Date );
		}

		public IEnumerable<Transaction> GetTransactionsBetweenDates(DateTime Start, DateTime End, int account)
		{
			var results = GetTransactions(t => t.Date >= Start && t.Date < End && t.AccountId == account && !t.Deleted);

			return results;
		}

		public Transaction GetFirstTransactionAfter(int account, DateTime start)
		{
			return _database.Connection.Table<Transaction>()
				.Where(t => t.AccountId == account && t.Date >= start && !t.Deleted)
				.OrderBy(t => t.Date)
				.Take(1)
				.FirstOrDefault();
		}

		public Transaction GetLastTransaction(int account)
		{
			return _database.Connection.Table<Transaction>()
				.Where(t => t.AccountId == account && !t.Deleted)
				.OrderByDescending(t => t.Date)
				.Take(1)
				.FirstOrDefault();
		}

		public void ImportTransactionBatch(string filename, int account)
		{
			var accountObject = _database.Connection.Get<Account>(account);

			if (accountObject == null)
			{
				throw new InvalidDataException($"Account {account} does not exist");
			}

			_database.Connection.Insert(new ImportBatch
			{
				Account = account,
				SourceFilename = filename,
				ImportTime = DateTime.UtcNow,
			});

			var batchId = (int)_database.GetLastInsertedRowId();

			_import.Import(filename, account, accountObject.Currency, batchId);
		}

		private Transaction PrepareDisplayTransaction(Transaction transaction)
		{
			transaction.Payee = transaction.Payee.Replace("&amp;", "&").Replace("&quot;", "\"");

			transaction.OriginalPayee = transaction.Payee;

			var reference = "";

			var match = SantanderRegex.Match(transaction.Payee);
			if (match.Success)
			{

				reference = SantanderRegex.Replace(transaction.Payee, SantanderOutputRef);
				transaction.Payee = SantanderRegex.Replace(transaction.Payee, SantanderOutput);
			}
			else
			{
				match = SantanderCashRegex.Match(transaction.Payee);
				if (match.Success)
				{

					reference = SantanderCashRegex.Replace(transaction.Payee, SantanderCashOutputRef);
					transaction.Payee = SantanderCashRegex.Replace(transaction.Payee, SantanderCashOutput);
				}
			}

			if (string.IsNullOrEmpty(transaction.Memo))
			{
				transaction.Memo = reference;
			}
			else
			{
				transaction.Reference = reference;
			}

			return transaction;
		}

		private Database _database;
		private Importer _import;

		private Regex SantanderRegex = new Regex(@"^(?:DIRECT DEBIT PAYMENT TO |CARD PAYMENT TO |STANDING ORDER VIA FASTER PAYMENT TO |BILL PAYMENT VIA FASTER PAYMENT TO |BANK GIRO CREDIT REF |CREDIT FROM |FASTER PAYMENTS RECEIPT REF)(?<Name>.*?)(?: (?:REF|REFERENCE) (?<Ref>[\w\- \/]+))?(?:,[\d\.]+ \w{2,4}, RATE [\d\.]+\/\w{2,4} ON \d{2}-\d{2}-\d{4})?(?:, MANDATE NO \d+)?(?:, MANDAT)?(?:, \d+\.\d{2})");
		private Regex SantanderCashRegex = new Regex(@"^CASH WITHDRAWAL AT (?<Name>[^,]+),.*$");
		private string SantanderOutput = "${Name}";
		private string SantanderOutputRef = "${Ref}";
		private string SantanderCashOutput = "CASH";
		private string SantanderCashOutputRef = "${Name}";
	}
}
