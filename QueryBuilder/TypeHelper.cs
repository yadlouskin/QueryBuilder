using System;
using System.Collections.Generic;
using System.Reflection;

namespace QueryBuilder
{
    /// <summary>
    /// Class to extract property and type names from PropertyInfo
    /// </summary>
    class PropertyHelper
    {
        private List<string> KnownTypes = new() { "String", "Int16", "Int32", "Boolean", "Date" };
        private Type TypeDefinition;
        private string TypeName;
        private string PropertyName;

        /// <summary>
        /// Constructor to handle information from PropertyInfo
        /// </summary>
        /// <param name="property">PropertyInfo instance</param>
        public PropertyHelper(PropertyInfo property)
        {
            PropertyName = property.Name;

            TypeDefinition = property.PropertyType;
            if (TypeDefinition.Name == "List`1")
            {
                TypeDefinition = TypeDefinition.GetGenericArguments()[0];
            }
            TypeName = TypeDefinition.Name;

            var EndsWithDate = (string s) =>
                !string.IsNullOrEmpty(s) && s.Length > 3
                && s.Substring(s.Length - 4).ToLower() == "date";

            if (TypeName == "Int32" && EndsWithDate(PropertyName))
            {
                TypeName = "Date";
            }
        }

        /// <summary>
        /// To check property is final or compound
        /// Final property has simple type like int, string, ...
        /// Compound property has type of custom class with other properties
        /// </summary>
        /// <returns>True if it is final property</returns>
        public bool IsSimpleProperty()
        {
            return KnownTypes.Contains(TypeName);
        }

        /// <summary>
        /// Get name of PropertyInfo type
        /// </summary>
        /// <returns>Name of type</returns>
        public string GetTypeName()
        {
            return TypeName;
        }

        /// <summary>
        /// Get name of property in class - this name is used for queries to db
        /// Name is compound, use parentName to return full name property
        /// </summary>
        /// <param name="parentName">Name of parent element of property</param>
        /// <returns>Full name of property</returns>
        public string GetPropertyName(string parentName = "")
        {
            if (string.IsNullOrEmpty(parentName))
            {
                return PropertyName;
            }
            return parentName + "." + PropertyName;
        }

        /// <summary>
        /// Get related properties if current property is compound
        /// </summary>
        /// <returns>Array of PropertyInfo of related properties</returns>
        public PropertyInfo[] GetRelatedProperties()
        {
            return IsSimpleProperty() ? new PropertyInfo[] { } : TypeDefinition.GetProperties();
        }
    }
}