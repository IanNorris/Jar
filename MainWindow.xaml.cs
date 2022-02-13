using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using Jar.DataModels;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;

namespace Jar
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		const string ProductName = "JarBudget";

		MessageBox _messageBox;
		DataModel _dataModel;

		static readonly Guid LocalLowGuid = new Guid("A520A1A4-1780-4FF6-BD18-167343C5AF16");

		private string GetKnownFolderPath(Guid knownFolderId)
		{
			IntPtr pszPath = IntPtr.Zero;
			try
			{
				int hr = SHGetKnownFolderPath(knownFolderId, 0, IntPtr.Zero, out pszPath);
				if (hr >= 0)
					return Marshal.PtrToStringAuto(pszPath);
				throw Marshal.GetExceptionForHR(hr);
			}
			finally
			{
				if (pszPath != IntPtr.Zero)
					Marshal.FreeCoTaskMem(pszPath);
			}
		}

		[DllImport("shell32.dll")]
		static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out IntPtr pszPath);

		public MainWindow(DataModel dataModel)
		{
			_messageBox = new MessageBox();
			_dataModel = dataModel;

			InitializeComponent();
		}

		[Conditional("DEBUG")]
		public void ShowDevTools()
		{
			m_browser.CoreWebView2.OpenDevToolsWindow();
		}

		private void m_browser_IsBrowserInitializedChanged(object sender, CoreWebView2InitializationCompletedEventArgs e)
		{
			var Base = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content");
			m_browser.CoreWebView2.SetVirtualHostNameToFolderMapping("jars.lh", Base, CoreWebView2HostResourceAccessKind.DenyCors);

			m_browser.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;

			m_browser.Source = new Uri("http://jars.lh/index.html");
			ShowDevTools();

			
		}

		private async void m_browser_Initialized(object sender, EventArgs e)
		{
			string LocalLowPath = GetKnownFolderPath(LocalLowGuid);
			var path = Path.Combine(LocalLowPath, ProductName);

			var environment = await CoreWebView2Environment.CreateAsync(null, path);
			await m_browser.EnsureCoreWebView2Async(environment);

			_messageBox.BindBrowser(async (code) =>
			{
				await m_browser.ExecuteScriptAsync(code);
			});
		}

		private async void m_browser_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
		{
			var message = JsonConvert.DeserializeObject<MessageWrapper>(e.WebMessageAsJson);
			var returnValue = _dataModel.OnMessageReceived(message);
			if(returnValue != null)
			{
				await m_browser.ExecuteScriptAsync($"callCallback({message.Callback}, {returnValue});");
			}
		}

		private async void m_browser_ContentLoading(object sender, CoreWebView2ContentLoadingEventArgs e)
		{
			await m_browser.ExecuteScriptAsync(_dataModel.CreateDataModelPayload() + "\n entryPoint();");
		}
	}
}
