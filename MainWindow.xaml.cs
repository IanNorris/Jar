using System;
using System.Diagnostics;
using System.IO;
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
		MessageBox _messageBox;
		DataModel _dataModel;

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
			await m_browser.EnsureCoreWebView2Async();

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
