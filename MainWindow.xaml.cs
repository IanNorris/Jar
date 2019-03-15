using CefSharp;
using CefSharp.Wpf;
using System.Diagnostics;
using System.Windows;

namespace Jar
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		DataModel m_dataModel;

		public MainWindow(DataModel dataModel)
		{
			m_dataModel = dataModel;

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

		const string ErrorMessageFormat = @"swal({{
				text: ""{0}"",
				title: ""{1}"",
				icon: ""{2}"",
				closeOnEsc: true,
				dangerMode: {3},
				buttons: {{
					cancel: {4},
					confirm: true
				}}
			}});";
		
		private static string BuildMessageCommand( string Text, string Title, MessageIcon Icon, bool ShowCancel, bool DangerMode )
		{
			return string.Format(ErrorMessageFormat, Text, Title, Icon.ToString().ToLower(), DangerMode.ToString().ToLower(), ShowCancel.ToString().ToLower());
		}

		private void ShowMessage(string Text, string Title, MessageIcon Icon, bool ShowCancel, bool DangerMode )
		{
			string JS = BuildMessageCommand(Text, Title, Icon, ShowCancel, DangerMode);
			m_browser.GetMainFrame().ExecuteJavaScriptAsync(JS);
		}

		private void m_browser_Loaded(object sender, RoutedEventArgs e)
		{
			m_browser.JavascriptObjectRepository.Register("dataModel", m_dataModel, true);
			m_dataModel.ShowMessage = ShowMessage;
		}
	}
}
