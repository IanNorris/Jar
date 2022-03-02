using System;
using System.Collections.Generic;
using JarPluginApi;

namespace JarPlugin.StarlingBank.Import
{
	class ImporterStarling : IImport
	{
		public ImporterStarling(IConfigService configService)
		{
			_configService = configService;
		}

		public ImportType Type()
		{
			return ImportType.Online;
		}

		public string[] Extensions()
		{
			throw new NotImplementedException();
		}

		public string FormatName()
		{
			return "Starling Bank";
		}

		public List<Transaction> Import(string Filename, int Account, int Currency, int BatchId)
		{
			throw new NotImplementedException();
		}

		private IConfigService _configService;
	}
}
