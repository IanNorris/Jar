
namespace JarPluginApi
{
	public abstract class IPluginRegistry<T> : IPluginRegistryBase
	{
		public Type BasePluginType => typeof(T);
		
		public void OnPluginLoadedInternal(object pluginInstance)
		{
			OnPluginLoaded((T)pluginInstance);
		}

		public abstract void OnPluginLoaded(T pluginInstance);
	}
}
