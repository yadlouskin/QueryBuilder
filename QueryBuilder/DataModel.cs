using System.Collections.Generic;
using MongoDB.Driver.Linq;

namespace QueryBuilder
{
    /// <summary>
    /// This class processes model classes and creates filters for query builder
    /// </summary>
    class DataModel
    {
        private List<string> Filters = new();

        /// <summary>
        /// Default constructor to create instance
        /// </summary>
        public DataModel()
        {
        }

        /// <summary>
        /// This function creates filters for query builder using model classes
        /// </summary>
        /// <returns>Filters as a string</returns>
        public string GetFilters()
        {
            TypeHelper typeCity = new(typeof(City));
            Dictionary<string, string> properties = typeCity.GetProperties();
            Dictionary<string, List<string>> classifiedProperties = typeCity.GetPropertiesByType();

            foreach (var property in properties)
            {
                string name = property.Key;
                string type = property.Value;
            }

            // Add stub filter to ensure the application works
            if (Filters.Count == 0)
            {
                return "{id: 'name',\n label: 'name',\n type: 'string'}";
            }
            return string.Join(",", Filters);
        }

    }

}