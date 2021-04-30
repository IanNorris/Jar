using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jar.Import
{
	public class Importer
	{
		private DataModel m_model;
		private Dictionary<string, IImport> m_importers = new Dictionary<string, IImport>();

		public Importer(DataModel Model)
		{
			m_model = Model;

			var ImportTypes = from AssemblyType in Assembly.GetExecutingAssembly().GetTypes()
								where AssemblyType.GetInterfaces().Contains(typeof(IImport))
								select Activator.CreateInstance(AssemblyType) as IImport;

			foreach( var Importer in ImportTypes )
			{
				var Extensions = Importer.Extensions();
				foreach( var Extension in Extensions )
				{
					var LowerExtension = Extension.ToLower();
					IImport Existing = null;

					if( m_importers.TryGetValue(LowerExtension, out Existing) )
					{
						throw new InvalidOperationException($"Already an importer for file type {LowerExtension}");
					}

					m_importers.Add(LowerExtension, Importer);
				}
			}
		}

		public void Import(string Filename, int Account, int Currency, int BatchId)
		{
			var Extension = Path.GetExtension(Filename).ToLower();

			IImport Importer = null;

			if (!m_importers.TryGetValue(Extension, out Importer))
			{
				throw new InvalidOperationException($"No importer for file type {Extension}");
			}

			Importer.Import(m_model, Filename, Account, Currency, BatchId);
		}
	}
}
