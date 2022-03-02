using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarPluginApi
{
	public interface IPluginRegistryBase
	{
		Type BasePluginType { get; }
		void OnPluginLoadedInternal(object pluginInstance);
	}
}
