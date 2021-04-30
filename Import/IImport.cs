namespace Jar.Import
{
	public interface IImport
	{
		string[] Extensions();
		string FormatName();
		void Import(DataModel Model, string Filename, int Account, int Currency, int BatchId);
	}
}
