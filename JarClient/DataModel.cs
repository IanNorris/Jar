using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Jar.DataModels;
using Jar.Import;
using Jar.Model;
using Jar.Services;
using JarPluginApi;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Jar.MessageBox;
using Settings = Jar.DataModels.Settings;

namespace Jar
{
	public class DataModel
	{
		public delegate void RegisterObjectDelegate(string name, object data);

		private Database _database;
		private EventBus _eventBus;

		public Settings Settings { get; private set; }
		public Accounts Accounts { get; private set; }
		public AccountCheckpoints AccountCheckpoints { get; private set; }
		public Transactions Transactions { get; private set; }
		public Budgets Budgets { get; private set; }
		public Configurations Configurations { get; private set; }

		public PluginService PluginService { get; private set; }

		public Importer Importer { get; private set; }

		private string _budgetName;

		public ShowMessageDelegate _showMessage;
		public WebBinding.ExecuteJavascriptDelegate _executeJS;

		public string GetBudgetName()
		{
			return _budgetName;
		}

		public void RunTemporaryDatabasePayload()
		{
#if DEBUG
			/*if(System.Windows.MessageBox.Show("Run debug payload?", "Debug", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes )
			{
				
			}*/
#endif
		}

		public async Task BuildDataModel(string Password)
		{
			Configurations.SetDatabase(_database, Password);

			RegisterPlugins();

			Accounts.SetDatabase(_database);
			AccountCheckpoints.SetDatabase(_database);
			Transactions.SetDatabase(_database);
			Budgets.SetDatabase(_database);

			var accounts = Accounts.GetAccounts();
			foreach (var account in accounts)
			{
				AccountCheckpoints.UpdateAccountCheckpoints(account.Id);
			}

			RunTemporaryDatabasePayload();

			foreach (var account in accounts)
			{
				if (account.OnlinePluginName != null)
				{
					await Transactions.ImportTransactionBatchOnline(account.Name, account.OnlinePluginName, account.Id);
				}
			}
		}

		public async Task<bool> CreateDatabase(string Filename, string Password)
		{
			_budgetName = Path.GetFileNameWithoutExtension(Filename);

			_database = new Database(_showMessage);
			if (_database.CreateDatabase(Filename, Password))
			{
				await BuildDataModel(Password);

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

			Settings = new Settings();
			Settings.ReadSettings(_showMessage, settingsPath);

			_eventBus = new EventBus();
			Importer = new Importer();
			Configurations = new Configurations(_eventBus);
			Accounts = new Accounts(_eventBus);
			Transactions = new Transactions(_eventBus, Importer);
			AccountCheckpoints = new AccountCheckpoints(Transactions, _eventBus);
			Budgets = new Budgets(_eventBus);
		}

		public void BindBrowser(WebBinding.ExecuteJavascriptDelegate executeJS)
		{
			_executeJS = executeJS;
		}

		public string CreateDataModelPayloadForFunction(string parentName, MethodInfo method)
		{
			var sb = new StringBuilder();

			var paramSig = string.Join(", ", method.GetParameters().Select(p => p.Name));
			sb.AppendLine($"async function( {paramSig} ) {{");

			var payload = string.Join(", ", method.GetParameters().Select(p => $"\"{p.Name}\": {p.Name}"));

			sb.AppendLine("\t\tlet res, rej;");
			sb.AppendLine("\t\tlet p = new Promise(function (ft, ff) { res = ft; rej = ff; });");

			sb.AppendLine($"\t\tsendEvent(\"{parentName}\", \"{method.Name}\", {{ {payload} }}, function (returnValue) {{");

			sb.AppendLine("\t\t\tres(returnValue);");
			sb.AppendLine("\t\t});");

			sb.AppendLine("\t\treturn await p;");
			sb.Append("\t}");

			return sb.ToString();
		}

		public string CreateDataModelPayload()
		{
			var sb = new StringBuilder();

			foreach (var property in GetType().GetProperties())
			{
				sb.AppendLine($"let {property.Name} = {{");

				bool firstMethod = true;
				foreach (var method in property.PropertyType.GetMethods())
				{
					if (!firstMethod)
					{
						sb.AppendLine(",\n");
					}
					else
					{
						firstMethod = false;
					}

					if (method.IsPublic)
					{
						sb.Append($"\t{method.Name}: ");
						sb.Append(CreateDataModelPayloadForFunction(property.Name, method));
					}
				}

				sb.AppendLine("\n};\n");
			}

			foreach (var localMethod in GetType().GetMethods())
			{
				if (localMethod.IsPublic)
				{
					sb.Append($"\tlet {localMethod.Name} = ");
					sb.Append(CreateDataModelPayloadForFunction("", localMethod));
					sb.AppendLine(";\n");
				}
			}

			return sb.ToString();
		}

		public async void InjectBindings(string CallbackName)
		{
			var payload = CreateDataModelPayload();
			await _executeJS(payload);
			await _executeJS($"{CallbackName}();");
		}

		public async Task<bool> OpenBudget(int BudgetIndex, string Path, string Password)
		{
			if (await CreateDatabase(Path, Password))
			{
				Settings.SetBudgetAsLatest(BudgetIndex);

				return true;
			}

			return false;
		}

		public async Task<bool> CreateNewBudget(string FilePath, string Password)
		{
			if (await CreateDatabase(FilePath, Password))
			{
				var NewBudget = new Budget();
				NewBudget.Path = FilePath;
				NewBudget.Name = Path.GetFileNameWithoutExtension(FilePath);
				NewBudget.LastAccessed = DateTime.UtcNow;
				NewBudget.RememberPassword = false;
				NewBudget.Password = Password;

				Settings.AddBudget(NewBudget);

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

				Settings.AddBudget(NewBudget);

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

		public async Task<string> CallMethodOnObject(object This, MethodInfo targetFunction, string payload)
		{
			var payloadReader = JObject.Parse(payload);

			var parameters = targetFunction.GetParameters();

			int parameterIndex = 0;
			object[] parametersOut = new object[parameters.Length];
			foreach (var parameter in parameters)
			{
				if (payloadReader.TryGetValue(parameter.Name, out var parameterToken))
				{
					var value = parameterToken.ToObject(parameter.ParameterType);
					parametersOut[parameterIndex++] = value;
				}
				else
				{
					throw new InvalidDataException($"{targetFunction.Name} function takes parameter {parameter.Name} but it is unspecified.");
				}
			}

			var returnValue = targetFunction.Invoke(This, parametersOut);
			if (targetFunction.ReturnType != typeof(void))
			{
				if(targetFunction.ReturnType.IsGenericType && targetFunction.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
				{
					await (returnValue as Task);

					var resultProperty = targetFunction.ReturnType.GetProperty("Result");
					var result = resultProperty.GetValue(returnValue);

					return JsonConvert.SerializeObject(result);
				}

				return JsonConvert.SerializeObject(returnValue);
			}
			else
			{
				return null;
			}
		}

		public async Task<string> OnMessageReceived(MessageWrapper message)
		{
			if (string.IsNullOrEmpty(message.Target))
			{
				var targetFunction = GetType().GetMethod(message.Function);
				if (targetFunction != null)
				{
					return await CallMethodOnObject(this, targetFunction, message.Payload);
				}
				else
				{
					throw new InvalidDataException($"{message.Function} function does not exist on data model or is not public.");
				}
			}

			var targetProperty = GetType().GetProperty(message.Target);

			if (targetProperty != null)
			{
				var targetFunction = targetProperty.PropertyType.GetMethod(message.Function);
				if (targetFunction != null && targetFunction.IsPublic)
				{
					return await CallMethodOnObject(targetProperty.GetValue(this), targetFunction, message.Payload);
				}
				else
				{
					throw new InvalidDataException($"{message.Function} function does not exist on {message.Target} or is not public.");
				}
			}
			else
			{
				throw new InvalidDataException($"{message.Target} is not a valid target interface");
			}
		}

		private void RegisterPlugins()
		{
			PluginService = new PluginService();

			AddPluginRegistries();

			var pluginNames = new HashSet<string>();

			PluginService.LoadPlugins(t =>
			{
				var pluginName = t.AssemblyQualifiedName.Split(',')[0];

				if(pluginNames.Contains(pluginName))
				{
					throw new InvalidDataException($"More than one plugin called {pluginName}");
				}
				pluginNames.Add(pluginName);

				var configService = new ConfigService(Configurations, pluginName);

				return new PluginContext(
					configService
				);
			});
		}

		private void AddPluginRegistries()
		{
			PluginService.AddPluginRegistry(Importer);
		}
	}
}
