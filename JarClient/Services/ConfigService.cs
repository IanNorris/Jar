using System.Collections.Generic;
using System.Linq;
using Jar.DataModels;
using JarPluginApi;

namespace Jar.Services
{
	public class ConfigService : IConfigService
	{
		public ConfigService(Configurations configurations, string PluginName, string AccountName)
		{
			_configurations = configurations;
			_pluginName = PluginName;
			_accountName = AccountName;
		}

		public IEnumerable<Configuration> GetConfigValues(string Name)
		{
			return _configurations.GetConfigurationValues(_pluginName, _accountName, Name);
		}

		public string GetConfigValue(string Name)
		{
			var values = _configurations.GetConfigurationValues(_pluginName, _accountName, Name);
			return values.FirstOrDefault()?.Value;
		}

		public void SetConfigValue(string Name, string Value, int ArrayIndex, bool IsCredential)
		{
			_configurations.UpsertConfiguration(_pluginName, _accountName, Name, ArrayIndex, Value, IsCredential);
		}

		public void SetConfigValue(string Name, string Value, bool IsCredential)
		{
			_configurations.UpsertConfiguration(_pluginName, _accountName, Name, 0, Value, IsCredential);
		}

		private string _pluginName;
		private string _accountName;
		private Configurations _configurations;
	}
}
