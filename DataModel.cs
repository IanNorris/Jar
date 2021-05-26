using System;
using System.IO;
using Microsoft.Win32;
using Jar.DataModels;
using static Jar.MessageBox;
using Jar.Model;
using Settings = Jar.DataModels.Settings;
using System.Threading.Tasks;

namespace Jar
{
	public class DataModel
	{
		public delegate void RegisterObjectDelegate(string name, object data);

		private Database _database;
		private EventBus _eventBus;

		private Settings _settings;
		private Accounts _accounts;
		private AccountCheckpoints _accountCheckpoints;
		private Transactions _transactions;

		private string m_budgetName;

		public Accounts GetAccounts() => _accounts;
		public AccountCheckpoints GetAccountCheckpoints() => _accountCheckpoints;
		public Transactions GetTransactions() => _transactions;


		public ShowMessageDelegate _showMessage;

		public void BuildDataModel()
		{
			_accounts.SetDatabase(_database);
			_accountCheckpoints.SetDatabase(_database);
			_transactions.SetDatabase(_database);

			foreach (var account in _accounts.GetAccounts())
			{
				_accountCheckpoints.UpdateAccountCheckpoints(account.Id);
			}
		}

		public bool CreateDatabase(string Filename, string Password)
		{
			m_budgetName = Path.GetFileNameWithoutExtension(Filename);
			
			_database = new Database(_showMessage);
			if (_database.CreateDatabase(Filename, Password))
			{
				BuildDataModel();

				return true;
			}

			return false;
		}


		public DataModel(string settingsPath)
		{
			//Temporary until the web UI is spun up
			_showMessage = (text, title, icon, showCancel, dangerMode) =>
			{
				System.Windows.MessageBox.Show(text, title, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
			};

			_settings = new Settings();
			_settings.ReadSettings(_showMessage, settingsPath);

			_eventBus = new EventBus();

			_accounts = new Accounts(_eventBus);
			_transactions = new Transactions(_eventBus);
			_accountCheckpoints = new AccountCheckpoints(_transactions, _eventBus);
		}

		public void RegisterObjects(RegisterObjectDelegate registerObject)
		{
			registerObject("dataModel", this);
			registerObject("settings", _settings);
			registerObject("accounts", _accounts);
			registerObject("accountCheckpoints", _accountCheckpoints);
			registerObject("transactions", _transactions);
		}

		public async Task<bool> OpenBudget( int BudgetIndex, string Path, string Password )
		{
			if (CreateDatabase(Path, Password))
			{
				_settings.SetBudgetAsLatest(BudgetIndex);

				return true;
			}

			return false;
		}

		public bool CreateNewBudget(string FilePath, string Password)
		{
			if(CreateDatabase(FilePath, Password))
			{
				var NewBudget = new Budget();
				NewBudget.Path = FilePath;
				NewBudget.Name = Path.GetFileNameWithoutExtension(FilePath);
				NewBudget.LastAccessed = DateTime.UtcNow;
				NewBudget.RememberPassword = false;
				NewBudget.Password = Password;

				_settings.AddBudget(NewBudget);

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

				_settings.AddBudget(NewBudget);

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

		public string GetBudgetName()
		{
			return m_budgetName;
		}

	}
}
