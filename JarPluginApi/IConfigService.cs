
namespace JarPluginApi
{
	public interface IConfigService
	{
		IEnumerable<Configuration> GetConfigValues(string Name);
		string GetConfigValue(string Name);
		void SetConfigValue(string Name, string Value, int ArrayIndex, bool IsCredential);
		void SetConfigValue(string Name, string Value, bool IsCredential);
	}
}
