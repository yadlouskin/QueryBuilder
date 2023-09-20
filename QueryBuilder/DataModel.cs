using System.Collections.Generic;

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
            // Add stub filter to ensure the application works
            if (Filters.Count == 0)
            {
                return @"{
                    id: 'name',
                    label: 'name',
                    type: 'string'
                }";
            }
            return string.Join(",", Filters);
        }
    }

}