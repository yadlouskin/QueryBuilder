using Microsoft.Extensions.Configuration;

namespace QueryBuilder
{
    /// <summary>
    /// Configuration class implemented as a Singleton
    /// Read stored in appsettings.json configuration
    /// 
    /// File appsettings_example.json is example of appsettings.json
    /// </summary>
    class Configuration
    {
        /// <summary>
        /// Url for QueryBuilder application
        /// Default value: "http://localhost:8000/"
        /// </summary>
        public string QueryBuilderUrl { get; private set; }

        /// <summary>
        /// Connection string to MongoDb
        /// Default value: ""
        /// </summary>
        public string MongoDbConnectionString { get; private set; }

        private Configuration()
        {
            // WARNING: File appsettings.json is not committed to repository due to security reasons, so you have to create this file
            // WARNING: Without appsettings.json file the following code can throw exception "File is not found"
            // TODO: We can use try..catch and assign default value, but in any case the default value will not work everywhere
            // TODO: We can add possibility to read values from environment variables or from command line arguments
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            QueryBuilderUrl = config.GetSection("ConnectionStrings")["QueryBuilderUrl"] ?? "http://localhost:8000/";
            MongoDbConnectionString = config.GetSection("ConnectionStrings")["MongoDbConnectionString"] ?? "";
        }

        private static Configuration? instance = null;

        /// <summary>
        /// Get configuration instance as a singletone
        /// </summary>
        /// <returns>Configuration instance</returns>
        public static Configuration GetConfiguration()
        {
            instance ??= new Configuration();
            return instance;
        }
    }
}