
namespace JarPluginApi
{
	public interface IConfigService
	{
		string GetConfigValue(string Name, int ArrayIndex);
		void DeleteConfigValue(string Name, int ArrayIndex);
		List<int> GetConfigValueIndices(string Name);
		void SetConfigValue(string Name, string Value, int ArrayIndex, bool IsCredential);
	}
}
