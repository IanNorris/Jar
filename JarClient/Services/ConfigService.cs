using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JarPluginApi;

namespace Jar.Services
{
	public class ConfigService : IConfigService
	{
		public ConfigService(InternalConfigService configService, string PluginName)
		{
			_configService = configService;
			_pluginName = PluginName;
		}

		public string GetConfigValue(string Name, int ArrayIndex)
		{
			return _configService.GetConfigValue(_pluginName, Name, ArrayIndex);
		}

		public void DeleteConfigValue(string Name, int ArrayIndex)
		{
			_configService.DeleteConfigValue(_pluginName, Name, ArrayIndex);
		}

		public List<int> GetConfigValueIndices(string Name)
		{
			return _configService.GetConfigValueIndices(_pluginName, Name);
		}

		public void SetConfigValue(string Name, string Value, int ArrayIndex, bool IsCredential)
		{
			_configService.SetConfigValue(_pluginName, Name, Value, ArrayIndex, IsCredential);
		}

		private string _pluginName;
		private InternalConfigService _configService;
	}
}
