using CefSharp;
using System;
using System.IO;

namespace Jar
{
	class LocalSchemeHandlerFactory : ISchemeHandlerFactory
	{
		public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
		{
			Uri u = new Uri(request.Url);
			String Base = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "content");
			String file = Path.Combine(Base, u.AbsolutePath.Substring(1));

			if (u.AbsolutePath.EndsWith("/"))
			{
				file += "\\index.html";
			}

			if (File.Exists(file))
			{
				Byte[] bytes = File.ReadAllBytes(file);
				var ResponseStream = new MemoryStream(bytes);
				var Type = "";
				switch (Path.GetExtension(file))
				{
					case ".html":
						Type = "text/html";
						break;
					case ".js":
						Type = "text/javascript";
						break;
					case ".css":
						Type = "text/css";
						break;
					case ".png":
						Type = "image/png";
						break;
					case ".appcache":
					case ".manifest":
						Type = "text/cache-manifest";
						break;
					default:
						Type = "application/octet-stream";
						break;
				}
				return ResourceHandler.FromStream(ResponseStream, Type, true);
			}
			else
			{
				return ResourceHandler.ForErrorMessage("Not found", System.Net.HttpStatusCode.NotFound);
			}
		}
	}
}
