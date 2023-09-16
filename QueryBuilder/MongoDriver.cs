using MongoDB.Driver;
using MongoDB.Bson;

namespace QueryBuilder
{
    /// <summary>
    /// Create connection to MongoDb
    /// Handle requests to DB and get information
    /// </summary>
    class MongoDriver
    {
        IMongoCollection<BsonDocument> Collection;

        /// <summary>
        /// MongoDriver constructor
        /// Create connection to MongoDb using setting from Configuration class
        /// </summary>
        /// <param name="database">Mongo database name. Left null to use name from config</param>
        /// <param name="collection">Collection name. Left null to use name from config</param>
        public MongoDriver(string? database = null, string? collection = null)
        {
            var config = Configuration.GetConfiguration();
            var client = new MongoClient(config.MongoUrl);
            Collection = client.GetDatabase(database ?? config.MongoDatabase)
                .GetCollection<BsonDocument>(collection ?? config.MongoCollection);
        }
    }
}