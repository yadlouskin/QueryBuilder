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

        /// <summary>
        /// Get data from MongoDb using conditions from json variable
        /// </summary>
        /// <param name="json">Variable with request conditions</param>
        /// <returns>String with data, returned from DB</returns>
        public string GetData(string json)
        {
            //var json = "{ SendId: 4, 'Events.Code' : { $all : [2], $nin : [3] } }";
            var result = Collection.Find(new QueryDocument(BsonDocument.Parse(json))).ToList();
            if (result.Count == 0)
            {
                return "0 items were found";
            }
            var str = "<ol><li>" + string.Join("</li><li>", result) + "</li></ol>";
            return str;
        }
    }
}