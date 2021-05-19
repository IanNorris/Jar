using System.Collections.Generic;
using Jar.Model;

namespace Jar.Import
{
	public interface IImport
	{
		string[] Extensions();
		string FormatName();
		List<Transaction> Import(string Filename, int Account, int Currency, int BatchId);
	}
}
