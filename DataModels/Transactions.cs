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

		public IEnumerable<Transaction> GetTransactions(Expression<Func<Transaction, bool>> Predicate)
		{
			return _database.Connection.Table<Transaction>().Where(Predicate).Select(t => PrepareDisplayTransaction(t));
		}

		public IEnumerable<Transaction> GetTransactionsBetweenDates(DateTime Start, DateTime End, int account)
		{
			var results = GetTransactions(t => t.Date >= Start && t.Date < End && t.AccountId == account && !t.Deleted).ToList();

			return results;
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
