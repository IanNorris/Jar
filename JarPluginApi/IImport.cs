﻿
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
		Task<List<Transaction>> Import(string AccountName, string Filename, int Account, int Currency, int BatchId, DateTime ImportFrom);
	}
}
