using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Jar
{
	public class Budget
	{
		public string Name = "";
		public string Path = "";
		public bool RememberPassword;
		public bool RememberedPassword;
		public DateTime LastAccessed = DateTime.UtcNow;

		[JsonIgnore]
		public string Password = "";
	}

	public class WindowSettings
	{
		public int X = -1;
		public int Y = -1;
		public int Width = -1;
		public int Height = -1;
		public bool Maximized = false;
		public float SizeNavSize = 0.0f;
	}

	public class Settings
	{
		public List<Budget> Budgets = new List<Budget>();
		public WindowSettings WindowSettings = new WindowSettings();
	}
}
