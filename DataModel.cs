using SQLite;
using Jar.Model;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Jar.Import;
using Newtonsoft.Json.Linq;
using System.Windows;
using Newtonsoft.Json;
using Microsoft.Win32;

namespace Jar
{
	public enum MessageIcon
	{
		Warning,
		Error,
		Success,
		Info
	}

	public class DataModel
	{
		public delegate void ShowMessageDelegate(string Text, string Title, MessageIcon Icon, bool ShowCancel, bool DangerMode);

		private SQLiteConnection m_database;
		private Settings m_settings;
		private Importer m_import;
		private string m_settingsPath;

		public SQLiteConnection Connection { get { return m_database; } }
		public Importer Import { get { return m_import; } }
		public Settings Settings { get { return m_settings; } }

		public ShowMessageDelegate ShowMessage;

		public SQLiteConnection CreateDatabase(string Path, string Password)
		{
			m_import = new Importer(this);

			var ConnectionString = new SQLiteConnectionString(
				Path,
				true,
				key: Password
			);

			try
			{
				m_database = new SQLiteConnection(ConnectionString);
				m_database.CreateTable<Transaction>();
			}
			catch(Exception)
			{
				if (ShowMessage != null)
				{
					ShowMessage("Invalid password, your budget is corrupted or you selected a file that is not a budget.", "Unable to open budget", MessageIcon.Error, false, false);
				}

				m_database = null;
				return null;
			}

			
			m_database.CreateTable<Account>();
			m_database.CreateTable<Transaction>();
			m_database.CreateTable<ImportBatch>();

			return m_database;
		}

		public void ImportTransactionBatch(string filename, int account)
		{
			var accountObject = m_database.Get<Account>(account);

			if(accountObject == null)
			{
				throw new InvalidDataException($"Account {account} does not exist");
			}

			m_database.Insert(new ImportBatch
			{
				Account = account,
				SourceFilename = filename,
				ImportTime = DateTime.UtcNow,
			});

			var batchId = (int)SQLite3.LastInsertRowid(m_database.Handle);

			m_import.Import(filename, account, accountObject.Currency, batchId);
		}

		public int CreateAccount( string name, AccountType type, int currency )
		{
			var lastAccountOrder = m_database.Table<Account>().Select(a => a.Order).DefaultIfEmpty(0).Max();

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

			m_database.Insert(newAccount);

			var accountId = (int)SQLite3.LastInsertRowid(m_database.Handle);

			return accountId;
		}

		public DataModel(string SettingsPath)
		{
			m_settingsPath = SettingsPath;
			m_settings = new Settings();

			if ( File.Exists(SettingsPath) )
			{
				try
				{
					var SettingsJson = JObject.Parse(File.ReadAllText(SettingsPath));

					bool HasBudgets = SettingsJson.ContainsKey("Budgets");
					bool HasWindowsSettings = SettingsJson.ContainsKey("Window");

					if(HasBudgets)
					{
						var JsonList = SettingsJson["Budgets"].Children().ToList();
						foreach(var JsonBudget in JsonList )
						{
							var Budget = JsonBudget.ToObject<Budget>();
							Budget.Name = Path.GetFileNameWithoutExtension(Budget.Path);
														
							m_settings.Budgets.Add(Budget);
						}
					}

					if(HasWindowsSettings)
					{
						m_settings.WindowSettings = SettingsJson["Window"].ToObject<WindowSettings>();
					}
				}
				catch(Exception e)
				{
					MessageBox.Show($"Settings file {SettingsPath} is corrupt.\nYou can attempt to load your budget again by opening it. Technical details:\n{e.Message}", "Error", MessageBoxButton.OK);
				}
			}

			m_settings.Budgets = m_settings.Budgets.OrderByDescending(b => b.LastAccessed ).ToList();

			WriteSettings();
		}

		public void WriteSettings()
		{
			var String = JsonConvert.SerializeObject(m_settings, Formatting.Indented);
			File.WriteAllText(m_settingsPath, String);
		}

		public Settings GetSettings()
		{
			return m_settings;
		}

		public bool OpenBudget( string Path, string Password )
		{
			return CreateDatabase(Path, Password) != null;
		}

		public bool CreateNewBudget(string FilePath, string Password)
		{
			if(CreateDatabase(FilePath, Password) != null )
			{
				var NewBudget = new Budget();
				NewBudget.Path = FilePath;
				NewBudget.Name = Path.GetFileNameWithoutExtension(FilePath);
				NewBudget.LastAccessed = DateTime.UtcNow;
				NewBudget.RememberPassword = false;
				NewBudget.Password = Password;

				m_settings.Budgets.Add(NewBudget);

				WriteSettings();

				return true;
			}

			return false;
		}

		public bool OpenExistingBudget()
		{
			OpenFileDialog Dialog = new OpenFileDialog();

			Dialog.Filter = "Budget (*.jar)|*.jar|All files (*.*)|*.*";
			Dialog.FilterIndex = 1;
			Dialog.RestoreDirectory = true;

			var Result = Dialog.ShowDialog();
			if (Result.GetValueOrDefault())
			{
				//Get the path of specified file
				var filePath = Dialog.FileName;

				var NewBudget = new Budget();
				NewBudget.Path = filePath;
				NewBudget.Name = Path.GetFileNameWithoutExtension(filePath);
				NewBudget.LastAccessed = DateTime.UtcNow;
				NewBudget.RememberPassword = false;

				Settings.Budgets.Add(NewBudget);

				return true;
			}

			return false;
		}

		public Budget GetNewBudgetPath()
		{
			SaveFileDialog Dialog = new SaveFileDialog();

			Dialog.Filter = "Budget (*.jar)|*.jar|All files (*.*)|*.*";
			Dialog.FilterIndex = 1;
			Dialog.RestoreDirectory = true;

			var Result = Dialog.ShowDialog();
			if (Result.GetValueOrDefault())
			{
				//Get the path of specified file
				var filePath = Dialog.FileName;

				var NewBudget = new Budget();
				NewBudget.Path = filePath;
				NewBudget.Name = Path.GetFileNameWithoutExtension(filePath);
				NewBudget.LastAccessed = DateTime.UtcNow;
				NewBudget.RememberPassword = false;

				return NewBudget;
			}

			return null;
		}

		public IEnumerable<Account> GetAccounts()
		{
			var results = m_database.Table<Account>().ToArray();
			foreach( var result in results )
			{
				result.LastBalance = m_database.Table<Transaction>().Where(t => t.Account == result.Id ).Select(t => t.Amount).Sum();
			}

			return results;
		}

		public IEnumerable<Transaction> GetTransactions()
		{
			return m_database.Table<Transaction>();
		}

		public IEnumerable<Transaction> GetTransactions(System.Linq.Expressions.Expression<Func<Transaction,bool>> Predicate)
		{
			return m_database.Table<Transaction>().Where(Predicate);
		}

		public IEnumerable<Transaction> GetTransactionsBetweenDates( DateTime Start, DateTime End, int account )
		{
			var results = GetTransactions(t => t.Date >= Start && t.Date < End && t.Account == account).ToList();

			return results;
		}
	}
}
