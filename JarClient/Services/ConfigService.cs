using System.Collections.Generic;
using System.Linq;
using Jar.DataModels;
using JarPluginApi;

namespace Jar.Services
{
	public class ConfigService : IConfigService
	{
		public ConfigService(Configurations configurations, string PluginName)
		{
			_configurations = configurations;
			_pluginName = PluginName;
		}

		public IEnumerable<Configuration> GetConfigValues(string AccountName, string Name)
		{
			return _configurations.GetConfigurationValues(_pluginName, AccountName, Name);
		}

		public string GetConfigValue(string AccountName, string Name)
		{
			var values = _configurations.GetConfigurationValues(_pluginName, AccountName, Name);
			return values.FirstOrDefault()?.Value;
		}

		public void SetConfigValue(string AccountName, string Name, string Value, int ArrayIndex, bool IsCredential)
		{
			_configurations.UpsertConfiguration(_pluginName, AccountName, Name, ArrayIndex, Value, IsCredential);
		}

		public void SetConfigValue(string AccountName, string Name, string Value, bool IsCredential)
		{
			_configurations.UpsertConfiguration(_pluginName, AccountName, Name, 0, Value, IsCredential);
		}

		private string _pluginName;
		private Configurations _configurations;
	}
}
