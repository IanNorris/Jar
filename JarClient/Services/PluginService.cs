using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JarPluginApi;

namespace Jar.Services
{
	public class PluginService
	{
		public delegate PluginContext OnPluginLoadedDelegate(Type pluginType);

		public PluginService()
		{

		}

		public void AddPluginRegistry(IPluginRegistryBase registry)
		{
			_registries.Add(registry);
		}

		public void LoadPlugins(OnPluginLoadedDelegate onPluginLoadedDelegate)
		{
			var mainAssembly = Assembly.GetExecutingAssembly();
			LoadPluginsForAssembly(mainAssembly, onPluginLoadedDelegate);

			var Plugins = Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "JarPlugin.*.dll");
			foreach (var Plugin in Plugins)
			{
				var assembly = Assembly.LoadFrom(Plugin);
				LoadPluginsForAssembly(assembly, onPluginLoadedDelegate);
			}
		}

		private object CreateInstanceWithDependencyInjection(Type type, PluginContext pluginContext)
		{
			var constructors = type.GetConstructors();
			foreach (var constructor in constructors.OrderByDescending(c => c.GetParameters().Length))
			{
				var parameters = constructor.GetParameters();

				bool success = true;
				int parameterIndex = 0;
				object[] parametersOut = new object[parameters.Length];
				foreach (var parameter in parameters)
				{
					foreach (var property in pluginContext.GetType().GetProperties())
					{
						if(parameter.ParameterType == property.PropertyType)
						{
							parametersOut[parameterIndex] = property.GetValue(pluginContext);
							break;
						}
					}

					if(parametersOut[parameterIndex] == null)
					{
						success = false;
						break;
					}
				}

				if(success)
				{
					return Activator.CreateInstance(type, parametersOut);
				}
			}

			throw new InvalidDataException($"Unable to create instance of {type.Name}, no suitable construtor that can be filled with dependency injection.");
		}

		private void LoadPluginsForAssembly(Assembly assembly, OnPluginLoadedDelegate onPluginLoadedDelegate)
		{
			var assemblyTypes = assembly.GetTypes();
			foreach (var registry in _registries)
			{
				var types = assemblyTypes.Where(t => t.GetInterfaces().Contains(registry.BasePluginType));
				foreach (var type in types)
				{
					var pluginContext = onPluginLoadedDelegate(type);

					var newInstance = CreateInstanceWithDependencyInjection(type, pluginContext);
					registry.OnPluginLoadedInternal(newInstance);
				}
			}
		}

		private List<IPluginRegistryBase> _registries = new List<IPluginRegistryBase>();
	}
}
