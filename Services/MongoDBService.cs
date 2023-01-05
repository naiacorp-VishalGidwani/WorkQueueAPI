using MongoDB.Driver;

namespace WorkQueueAPI.Services
{
    public class MongoDBService
    {
        private IMongoDatabase db;

        public MongoDBService(IConfiguration config)
        {
            String connectionString = config.GetValue<String>("ConnectionString");
            String databaseName = config.GetValue<String>("DatabaseName");

            MongoClient client = new MongoClient(
                connectionString
            );

            db = client.GetDatabase(databaseName);
        }

        public IMongoDatabase GetDB()
        {
            return db;
        }

    }
}
