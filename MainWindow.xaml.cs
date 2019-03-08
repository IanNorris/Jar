using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Jar
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
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
	}
}
