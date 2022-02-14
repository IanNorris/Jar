
namespace JarPluginApi
{
	public enum ImportType
	{
		Online,
		File
	}

	public interface IImport
	{
		ImportType Type();
		string[] Extensions();
		string FormatName();
		List<Transaction> Import(string Filename, int Account, int Currency, int BatchId);
	}
}
