using CefSharp;
using CefSharp.Wpf;
using System.Diagnostics;
using System.Windows;
using SQLite;

namespace Jar
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		DataModel m_database;

		public MainWindow(DataModel database)
		{
			m_database = database;

			var Settings = new CefSettings();
			Settings.RegisterScheme(
				new CefCustomScheme
				{
					SchemeName = "local",
					SchemeHandlerFactory = new LocalSchemeHandlerFactory()
				}
			);

			Cef.Initialize(Settings);

			InitializeComponent();
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
				m_browser.Load($@"local://index.html");

				ShowDevTools();
			}
		}

		private void m_browser_Loaded(object sender, RoutedEventArgs e)
		{
			m_browser.JavascriptObjectRepository.Register("data", m_database, true);
		}
	}
}
