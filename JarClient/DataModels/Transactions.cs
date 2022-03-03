using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jar.Import;
using Jar.Model;
using JarPluginApi;
using Newtonsoft.Json;

namespace Jar.DataModels
{
	public class Transactions
	{
		public Transactions(EventBus eventBus, Importer importer)
		{
			_import = importer;

			LoadInstitutions();
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

			foreach (var transaction in preTransactions)
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

			foreach (var result in results)
			{
				if (result.CheckpointId != currentCheckpointId)
				{
					if (result.Balance != previousTotal && previousTotal != long.MinValue)
					{
						throw new InvalidDataException($"Balance pre transaction for #{result.Id} of {result.Balance} does not match previous sum of {previousTotal}.");
					}

					runningTotal = 0;
					currentCheckpointId = result.CheckpointId;
				}

				runningTotal += result.Amount;

				var newBalance = runningTotal + result.Balance;
				result.Balance = newBalance;

				PrepareDisplayTransaction(result);

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
			return _database.Connection.Table<Transaction>().Where(predicate).OrderBy(t => t.Date);
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

		public async Task ImportTransactionBatchFromFile(string accountName, string filename, int account)
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

			var transactions = await _import.ImportFile(accountName, filename, account, accountObject.Currency, batchId);
		}

		public async Task ImportTransactionBatchOnline(string accountName, string pluginName, int account)
		{
			var accountObject = _database.Connection.Get<Account>(account);

			if (accountObject == null)
			{
				throw new InvalidDataException($"Account {account} does not exist");
			}

			_database.Connection.Insert(new ImportBatch
			{
				Account = account,
				SourceFilename = pluginName,
				ImportTime = DateTime.UtcNow,
			});

			var batchId = (int)_database.GetLastInsertedRowId();

			var transactions = await _import.ImportOnline(accountName, pluginName, account, accountObject.Currency, batchId);
		}

		private void PrepareDisplayTransaction(Transaction transaction)
		{
			transaction.OriginalPayee = transaction.Payee;
			transaction.Payee = transaction.Payee.Replace("&amp;", "&").Replace("&quot;", "\"").Replace("\uFFFD", "");

			var reference = "";

			var finished = false;
			foreach (var institution in Institutions)
			{
				foreach (var filter in institution.Value.Filters)
				{
					var match = filter.CompiledRegex.Match(transaction.Payee);
					if (match.Success)
					{
						reference = filter.CompiledRegex.Replace(transaction.Payee, filter.ReferenceOutput).Trim();
						transaction.Payee = filter.CompiledRegex.Replace(transaction.Payee, filter.PayeeOutput).Trim();
						finished = true;
						break;
					}
				}

				if (finished)
				{
					break;
				}
			}

			if (string.IsNullOrEmpty(transaction.Memo))
			{
				transaction.Memo = reference.Trim();
			}
			else
			{
				transaction.Reference = reference.Trim();
			}
		}

		private void LoadInstitutions()
		{
			var path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Institutions.json");
			var jsonContent = File.ReadAllText(path);
			Institutions = JsonConvert.DeserializeObject<Dictionary<string, Institution>>(jsonContent);

			foreach (var institution in Institutions)
			{
				foreach (var filter in institution.Value.Filters)
				{
					filter.CompiledRegex = new Regex(filter.Regex, RegexOptions.Compiled);
				}
			}
		}

		private Database _database;
		private Importer _import;

		private Dictionary<string, Institution> Institutions;
	}
}
