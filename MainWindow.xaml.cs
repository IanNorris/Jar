using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using CefSharp;
using CefSharp.Wpf;

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

			CefSharpSettings.ConcurrentTaskExecution = true;

			var Settings = new CefSettings()
			{
				Locale = System.Globalization.CultureInfo.CurrentCulture.Name
			};
			Settings.RegisterScheme(
				new CefCustomScheme
				{
					SchemeName = "local",
					SchemeHandlerFactory = new LocalSchemeHandlerFactory()
				}
			);

			Cef.Initialize(Settings);
			

			_messageBox.BindBrowser(m_browser);

			InitializeComponent();

			m_browser.JavascriptObjectRepository.NameConverter = null;
		}

		[Conditional("DEBUG")]
		public void ShowDevTools()
		{
			m_browser.ShowDevTools();
		}

		private void m_browser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if ((bool)e.NewValue)
			{
				m_browser.MenuHandler = new MenuHandler();

				m_browser.Load($@"local://index.html");

				ShowDevTools();
			}
		}

		

		private void m_browser_Loaded(object sender, RoutedEventArgs e)
		{
			_dataModel._showMessage = _messageBox.ShowMessage;

			_dataModel.RegisterObjects((name, obj) =>
			{
				m_browser.JavascriptObjectRepository.Register(name, obj);
			});
		}
	}
}
