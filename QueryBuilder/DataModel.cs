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
                List<string> comparisonProperties = classifiedProperties[type];
                if (type == "Int16" || type == "Int32")
                {
                    // Add possibility to compare short and int properties in UI
                    // Check that we have such properties in our dictionary
                    string otherType = type == "Int16" ? "Int32" : "Int16";
                    if (classifiedProperties.ContainsKey(otherType))
                    {
                        comparisonProperties.AddRange(classifiedProperties[otherType]);
                    }
                }
                GenerateFilterWithOptions(name, type, comparisonProperties);
                if (type == "Date")
                {
                    GenerateFilterForDate(name, type);
                }
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
            string filter = string.Format("{{\n id: '{0}_val'"
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
                    filter += ",\n validation: {max: 32767, min:-32768}";
                    break;

                case "Bool":
                    filter += ",\n type: 'boolean'";
                    break;

                case "String":
                    break;

                case "Date":
                    string standardFieldName = " field: '" + name + "'";
                    string dateFieldName = " field: '" + name + "_optDateToUseExpr'";
                    filter = filter.Replace(standardFieldName, dateFieldName);
                    filter += ",\n type: 'string'";
                    filter += ", \n operators: " +
                        "['equal','not_equal','less','less_or_equal','greater','greater_or_equal']";
                    break;

                default:
                    filter += ",\n type: 'string'";
                    break;
            }

            filter += "\n}";
            Filters.Add(filter);
            return true;
        }

        private bool GenerateFilterWithOptions(string name, string type, List<string> options)
        {
            string filter = string.Format("{{\n id: '{0}_opt'"
                + ",\n field: '{0}_optFirstToUseExpr'"
                + ",\n label: '{0}'"
                , name);

            filter += ",\n type: 'string'";
            filter += ",\n input: 'select'";
            filter += ",\n values: { \n";
            foreach(string value in options)
            {
                filter += string.Format("'{0}_optSecondToUseExpr':'{0}',\n", value);
            }
            filter = filter.Remove(filter.Length - 2, 2);

            filter += "\n}";
            filter += ", \n operators: " +
                "['equal','not_equal','less','less_or_equal','greater','greater_or_equal']";
            filter += "\n}";
            Filters.Add(filter);
            return true;
        }

        private bool GenerateFilterForDate(string name, string type)
        {
            if (type != "Date")
            {
                return false;
            }
            string filter = string.Format("{{\n id: '{0}_dat'"
                + ",\n field: '{0}'"
                + ",\n label: '{0}'"
                , name);

            filter += ",\n type: 'integer'";
            filter += ",\n input: 'select'";
            filter += ",\n values: { \n";

            var getdate = (DateTime date) => date.Year * 10000 + date.Month * 100 + date.Day;
            filter += string.Format("{0}:'Today'", getdate(DateTime.Today));
            filter += string.Format(",\n {0}:'Yesterday'", getdate(DateTime.Today.AddDays(-1)));
            filter += string.Format(",\n {0}:'Week ago'", getdate(DateTime.Today.AddDays(-7)));
            filter += string.Format(",\n {0}:'Month ago'", getdate(DateTime.Today.AddMonths(-1)));
            filter += string.Format(",\n {0}:'Year ago'", getdate(DateTime.Today.AddYears(-1)));

            filter += "\n}";
            filter += "\n}";
            Filters.Add(filter);
            return true;
        }

    }

}