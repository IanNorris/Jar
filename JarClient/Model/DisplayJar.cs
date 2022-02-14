using Jar.DataModels;

namespace Jar.Model
{
	public class DisplayJar : JarEntry
	{
		public Jar Jar { get; set; }

		public long PreviousValue { get; set; }
		public long AverageValue { get; set; }

		public long JarTotalToNext { get; set; }
	}
}
