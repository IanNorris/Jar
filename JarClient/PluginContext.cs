
using JarPluginApi;

namespace Jar
{
	public class PluginContext
	{
		public PluginContext(IConfigService configService)
		{
			ConfigService = configService;
		}

		public IConfigService ConfigService { get; private set; }
	}
}
