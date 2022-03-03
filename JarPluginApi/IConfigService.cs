
namespace JarPluginApi
{
	public interface IConfigService
	{
		IEnumerable<Configuration> GetConfigValues(string AccountName, string Name);
		string GetConfigValue(string AccountName, string Name);
		void SetConfigValue(string AccountName, string Name, string Value, int ArrayIndex, bool IsCredential);
		void SetConfigValue(string AccountName, string Name, string Value, bool IsCredential);
	}
}
