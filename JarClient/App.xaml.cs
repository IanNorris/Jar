using System;
using System.IO;
using System.Windows;
using Sodium;

namespace Jar
{
	public partial class App : Application
	{
		DataModel m_dataModel;

		public App()
		{
			const string AppDataName = "JarBudgeting";
			const string Settings = "Settings.json";

			SodiumCore.Init();

			var AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppDataName);
			Directory.CreateDirectory(AppDataPath);

			var SettingsPath = Path.Combine(AppDataPath, Settings);
			m_dataModel = new DataModel(SettingsPath);
		}

		private void Application_Startup(object sender, StartupEventArgs e)
		{
			MainWindow main = new MainWindow(m_dataModel);
			main.Show();
		}
	}
}
