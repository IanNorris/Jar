using SQLite;
using System;
using System.IO;
using System.Windows;

namespace Jar
{
	public partial class App : Application
	{
		DataModel m_database;

		public App()
		{
			const string AppDataName = "JarBudgeting";
			const string DBName = "Jar.db";

			var AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppDataName);
			Directory.CreateDirectory(AppDataPath);

			var DatabasePath = Path.Combine(AppDataPath, DBName);

			m_database = new DataModel(DatabasePath, "Hello");
		}

		private void Application_Startup(object sender, StartupEventArgs e)
		{
			MainWindow main = new MainWindow(m_database);
			main.Show();
		}
	}
}
