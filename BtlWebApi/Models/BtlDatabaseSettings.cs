namespace BtlWebApi.Models
{
    public class BtlDatabaseSettings : IBtlDatabaseSettings
    {
        public string ItemsCollectionName { get; set; }
        public string UsersCollectionName { get; set; }
        public string CodesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IBtlDatabaseSettings
    {
        string ItemsCollectionName { get; set; }
        string UsersCollectionName { get; set; }
        string CodesCollectionName { get; set; }

        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
