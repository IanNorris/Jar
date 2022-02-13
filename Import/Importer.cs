using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Jar.Model;

namespace Jar.Import
{
	public class Importer
	{
		private Dictionary<string, IImport> m_importers = new Dictionary<string, IImport>();

		public Importer()
		{
			var ImportTypes = from AssemblyType in Assembly.GetExecutingAssembly().GetTypes()
							  where AssemblyType.GetInterfaces().Contains(typeof(IImport))
							  select Activator.CreateInstance(AssemblyType) as IImport;

			foreach (var Importer in ImportTypes)
			{
				var Extensions = Importer.Extensions();
				foreach (var Extension in Extensions)
				{
					var LowerExtension = Extension.ToLower();
					IImport Existing = null;

					if (m_importers.TryGetValue(LowerExtension, out Existing))
					{
						throw new InvalidOperationException($"Already an importer for file type {LowerExtension}");
					}

					m_importers.Add(LowerExtension, Importer);
				}
			}
		}

		public List<Transaction> Import(string Filename, int Account, int Currency, int BatchId)
		{
			var Extension = Path.GetExtension(Filename).ToLower();

			IImport Importer = null;

			if (!m_importers.TryGetValue(Extension, out Importer))
			{
				throw new InvalidOperationException($"No importer for file type {Extension}");
			}

			return Importer.Import(Filename, Account, Currency, BatchId);
		}
	}
}
