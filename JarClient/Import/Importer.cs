using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using JarPluginApi;

namespace Jar.Import
{
	public class Importer : IPluginRegistry<IImport>
	{
		private Dictionary<string, IImport> m_fileImporters = new Dictionary<string, IImport>();
		private Dictionary<string, IImport> m_onlineImporters = new Dictionary<string, IImport>();

		public Importer()
		{
		}

		public async Task<List<Transaction>> ImportOnline(string AccountName, string PluginName, int Account, int Currency, int BatchId, DateTime ImportFrom)
		{
			if (!m_onlineImporters.TryGetValue(PluginName, out var importer))
			{
				throw new InvalidOperationException($"No importer matching plugin {PluginName}");
			}

			return await importer.Import(AccountName, null, Account, Currency, BatchId, ImportFrom);
		}

		public async Task<List<Transaction>> ImportFile(string AccountName, string Filename, int Account, int Currency, int BatchId, DateTime ImportFrom)
		{
			var Extension = Path.GetExtension(Filename).ToLower();

			if (!m_fileImporters.TryGetValue(Extension, out var importer))
			{
				throw new InvalidOperationException($"No importer for file type {Extension}");
			}

			return await importer.Import(AccountName, Filename, Account, Currency, BatchId, ImportFrom);
		}

		public override void OnPluginLoaded(IImport importer)
		{
			if (importer.Type() == ImportType.File)
			{
				var Extensions = importer.Extensions();
				foreach (var Extension in Extensions)
				{
					var LowerExtension = Extension.ToLower();
					IImport Existing = null;

					if (m_fileImporters.TryGetValue(LowerExtension, out Existing))
					{
						throw new InvalidOperationException($"Already an importer for file type {LowerExtension}");
					}

					m_fileImporters.Add(LowerExtension, importer);
				}
			}
			else if (importer.Type() == ImportType.Online)
			{
				m_onlineImporters.Add(importer.GetType().AssemblyQualifiedName.Split(',')[0], importer);
			}
		}
	}
}
