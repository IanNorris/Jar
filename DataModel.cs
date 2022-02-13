using System;
using System.IO;
using Microsoft.Win32;
using Jar.DataModels;
using static Jar.MessageBox;
using Jar.Model;
using Settings = Jar.DataModels.Settings;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Linq;
using System.Reflection;

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

		public string BudgetName { get; private set; }

		public ShowMessageDelegate _showMessage;

		public void BuildDataModel()
		{
			Accounts.SetDatabase(_database);
			AccountCheckpoints.SetDatabase(_database);
			Transactions.SetDatabase(_database);
			Budgets.SetDatabase(_database);

			foreach (var account in Accounts.GetAccounts())
			{
				AccountCheckpoints.UpdateAccountCheckpoints(account.Id);
			}
		}

		public bool CreateDatabase(string Filename, string Password)
		{
			BudgetName = Path.GetFileNameWithoutExtension(Filename);
			
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

			Settings = new Settings();
			Settings.ReadSettings(_showMessage, settingsPath);

			_eventBus = new EventBus();
			Accounts = new Accounts(_eventBus);
			Transactions = new Transactions(_eventBus);
			AccountCheckpoints = new AccountCheckpoints(Transactions, _eventBus);
			Budgets = new Budgets(_eventBus);
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

			foreach(var property in GetType().GetProperties())
			{
				sb.AppendLine($"let {property.Name} = {{");

				bool firstMethod = true;
				foreach(var method in property.PropertyType.GetMethods())
				{
					if(!firstMethod)
					{
						sb.AppendLine(",\n");
					}
					else
					{
						firstMethod = false;
					}

					if(method.IsPublic)
					{
						sb.Append($"\t{method.Name}: ");
						sb.Append(CreateDataModelPayloadForFunction(property.Name, method));
					}
				}

				sb.AppendLine("\n};\n");
			}

			foreach(var localMethod in GetType().GetMethods())
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

		public async Task<bool> OpenBudget( int BudgetIndex, string Path, string Password )
		{
			if (CreateDatabase(Path, Password))
			{
				Settings.SetBudgetAsLatest(BudgetIndex);

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

		public string GetBudgetName()
		{
			return BudgetName;
		}

		public string CallMethodOnObject(object This, MethodInfo targetFunction, string payload)
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
				return JsonConvert.SerializeObject(returnValue);
			}
			else
			{
				return null;
			}
		}

		public string OnMessageReceived(MessageWrapper message)
		{
			if(string.IsNullOrEmpty(message.Target))
			{
				var targetFunction = GetType().GetMethod(message.Function);
				if (targetFunction != null)
				{
					return CallMethodOnObject(this, targetFunction, message.Payload);
				}
				else
				{
					throw new InvalidDataException($"{message.Function} function does not exist on data model or is not public.");
				}
			}

			var targetProperty = GetType().GetProperty(message.Target);

			if(targetProperty != null)
			{
				var targetFunction = targetProperty.PropertyType.GetMethod(message.Function);
				if(targetFunction != null && targetFunction.IsPublic)
				{
					return CallMethodOnObject(targetProperty.GetValue(this), targetFunction, message.Payload);
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
	}
}
