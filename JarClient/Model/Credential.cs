using SQLite;

namespace Jar.Model
{
    public class Credential
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Plugin { get; set; }

        public string Account { get; set; }

        public string Value { get; set; }
    }
}
