using SQLite;

namespace Jar.Model
{
	public class Category
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public string Name { get; set; }
	}
}
