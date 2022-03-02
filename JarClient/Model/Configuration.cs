using SQLite;

namespace Jar.Model
{
    public class Configuration
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int ArrayIndex { get; set; }

        public string Name { get; set; }

        public string Plugin { get; set; }

        public string Account { get; set; }

        //The value here is re-encrypted with the master password
        //so if the database is ever dumped (accidentally or on purpose)
        //then the credential is not in plain text.
        public string Value { get; set; }

        public bool IsCredential { get; set; }
    }
}
