namespace StudentMessagingApp.Models
{
    public class DbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string Databasename { get; set; } = null!;
        public string StudentsCollectionName { get; set; } = null!;
        public string MessagesCollectionName { get; set; } = null!;
    }
}
