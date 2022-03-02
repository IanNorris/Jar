using System;
using System.Collections.Generic;
using System.IO;
using JarPluginApi;

namespace Jar.Import
{
	public class Importer : IPluginRegistry<IImport>
	{
		private Dictionary<string, IImport> m_importers = new Dictionary<string, IImport>();

		public Importer()
		{
		}

		public List<Transaction> ImportFile(string Filename, int Account, int Currency, int BatchId)
		{
			var Extension = Path.GetExtension(Filename).ToLower();

			if (!m_importers.TryGetValue(Extension, out var importer))
			{
				throw new InvalidOperationException($"No importer for file type {Extension}");
			}

			return importer.Import(Filename, Account, Currency, BatchId);
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

					if (m_importers.TryGetValue(LowerExtension, out Existing))
					{
						throw new InvalidOperationException($"Already an importer for file type {LowerExtension}");
					}

					m_importers.Add(LowerExtension, importer);
				}
			}
			else
			{

			}
		}
	}
}
