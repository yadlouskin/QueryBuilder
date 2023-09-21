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
                GenerateFilter(name, type);
            }

            // Add stub filter to ensure the application works
            if (Filters.Count == 0)
            {
                return "{id: 'name',\n label: 'name',\n type: 'string'}";
            }
            return string.Join(",", Filters);
        }

        private bool GenerateFilter(string name, string type)
        {
            string filter = string.Format("{{\n id: '{0}'"
                + ",\n field: '{0}'"
                + ",\n label: '{0}'"
                , name);

            // TODO: Add field and create different filters with the same field
            switch (type)
            {
                case "Int32":
                    filter += ",\n type: 'integer'";
                    break;

                case "Int16":
                    filter += ",\n type: 'integer'";
                    break;

                case "Bool":
                    filter += ",\n type: 'boolean'";
                    break;

                case "String":
                    break;

                case "Date":
                    filter += ",\n type: 'date'";
                    filter += ",\n validation: {format: 'YYYYMMDD'}";
                    filter += ",\n plugin: 'datepicker'";
                    filter += ",\n plugin_config: {\n format: 'yyyymmdd', \n" +
                        "todayBtn: 'linked',\n todayHighlight: true,\n autoclose: true \n}";
                    break;

                default:
                    filter += ",\n type: 'string'";
                    break;
            }

            filter += "\n}";
            Filters.Add(filter);
            return true;
        }

    }

}