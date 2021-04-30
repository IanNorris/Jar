using System;
using SQLite;

namespace Jar.Model
{
	class ImportBatch
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public int Account { get; set; }
		public DateTime ImportTime { get; set; }
		public string SourceFilename { get; set; }
	}
}