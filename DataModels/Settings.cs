using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Jar.MessageBox;

namespace Jar.DataModels
{
	public class Settings
	{
		public void ReadSettings(ShowMessageDelegate showMessage, string settingsPath)
		{
			_settingsPath = settingsPath;
			_settings = new Model.Settings();

			if (File.Exists(settingsPath))
			{
				try
				{
					var SettingsJson = JObject.Parse(File.ReadAllText(settingsPath));

					bool HasBudgets = SettingsJson.ContainsKey("Budgets");
					bool HasWindowsSettings = SettingsJson.ContainsKey("WindowSettings");

					if (HasBudgets)
					{
						var JsonList = SettingsJson["Budgets"].Children().ToList();
						foreach (var JsonBudget in JsonList)
						{
							var Budget = JsonBudget.ToObject<Model.Budget>();
							Budget.Name = Path.GetFileNameWithoutExtension(Budget.Path);

							_settings.Budgets.Add(Budget);
						}
					}

					if (HasWindowsSettings)
					{
						_settings.WindowSettings = SettingsJson["WindowSettings"].ToObject<Model.WindowSettings>();
					}
				}
				catch (Exception e)
				{
					showMessage($"Settings file {settingsPath} is corrupt.\nYou can attempt to load your budget again by opening it. Technical details:\n{e.Message}", "Error", MessageIcon.Error, false, true);
				}
			}

			_settings.Budgets = _settings.Budgets.OrderByDescending(b => b.LastAccessed).ToList();

			WriteSettings();
		}

		public void WriteSettings()
		{
			var String = JsonConvert.SerializeObject(_settings, Formatting.Indented);
			File.WriteAllText(_settingsPath, String);
		}

		public Model.Settings GetSettings()
		{
			return _settings;
		}

		public List<Model.Budget> GetBudgets()
		{
			return _settings.Budgets;
		}

		public double GetSideNavWidth()
		{
			return _settings.WindowSettings.SizeNavSize;
		}

		public void SetSideNavWidth(double newSize)
		{
			_settings.WindowSettings.SizeNavSize = (float)newSize;

			WriteSettings();
		}

		public void SetBudgetAsLatest(int budgetIndex)
		{
			_settings.Budgets[budgetIndex].LastAccessed = DateTime.UtcNow;

			WriteSettings();
		}

		public void AddBudget(Model.Budget budget)
		{
			_settings.Budgets.Add(budget);

			WriteSettings();
		}

		private Model.Settings _settings;

		private string _settingsPath;
	}
}
