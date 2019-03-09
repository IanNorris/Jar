using SQLite;
using System;
using System.IO;
using System.Windows;

namespace Jar
{
	public partial class App : Application
	{
		SQLiteConnection m_database;

		public App()
		{
			const string AppDataName = "JarBudgeting";
			const string DBName = "Jar.db";

			var AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppDataName);
			Directory.CreateDirectory(AppDataPath);

			var DatabasePath = Path.Combine(AppDataPath, DBName);

			DataModel.Create(DatabasePath, "Hello");
		}
	}
}
