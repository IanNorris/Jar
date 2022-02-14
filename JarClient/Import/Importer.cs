using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JarPluginApi;

namespace Jar.Import
{
	public class Importer
	{
		private Dictionary<string, IImport> m_importers = new Dictionary<string, IImport>();

		public Importer()
		{
			var mainAssembly = Assembly.GetExecutingAssembly();
			LoadImportersForAssembly(mainAssembly);

			var Plugins = Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "JarPlugin.*.dll");
			foreach (var Plugin in Plugins)
			{
				var assembly = Assembly.LoadFrom(Plugin);
				LoadImportersForAssembly(assembly);
			}
		}

		private void LoadImportersForAssembly(Assembly assembly)
		{
			var ImportTypes = from AssemblyType in assembly.GetTypes()
							  where AssemblyType.GetInterfaces().Contains(typeof(IImport))
							  select Activator.CreateInstance(AssemblyType) as IImport;

			foreach (var Importer in ImportTypes)
			{
				if (Importer.Type() == ImportType.File)
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
				else
				{

				}
			}
		}

		public List<Transaction> ImportFile(string Filename, int Account, int Currency, int BatchId)
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
