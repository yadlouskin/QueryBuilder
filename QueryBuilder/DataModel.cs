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

            classifiedProperties.Add("Integer", new List<string>());
            List<string> integerTypes = new List<string> {"Int16", "Int32", "Decimal"};
            foreach(string type in integerTypes)
            {
                if (classifiedProperties.ContainsKey(type))
                {
                    classifiedProperties["Integer"].AddRange(classifiedProperties[type]);
                }
            }

            foreach (var property in properties)
            {
                string name = property.Key;
                string type = property.Value;
                GenerateFilter(name, type);
                List<string> comparisonProperties = classifiedProperties[type];
                if (integerTypes.Contains(type))
                {
                    // Add possibility to compare short, int and decimal properties in UI
                    comparisonProperties = classifiedProperties["Integer"];
                }
                GenerateFilterWithOptions(name, type, comparisonProperties);
                if (type == "Date")
                {
                    GenerateFilterForDate(name, type);
                }
                if (integerTypes.Contains(type))
                {
                    GenerateFilterForExpression(name, type);
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

                case "Decimal":
                    filter += ",\n type: 'double'";
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

        private bool GenerateFilterForExpression(string name, string type)
        {
            if (type != "Int16" && type != "Int32" && type != "Decimal")
            {
                return false;
            }
            string filter = string.Format("{{\n id: '{0}_exp'"
                + ",\n field: '$expr'"
                + ",\n label: '{0}'"
                , name);

            filter += ",\n type: 'string'";
            filter += @",
valueGetter: function(rule) {
    var ruleFilter = rule.id + '_filter';
    var ruleFilterOperator = ruleFilter + '_operator';
    var ruleFilterSecond = ruleFilter + '_second';
    var ruleComparisonOperator = rule.id + '_operator_second';
    var ruleValue = rule.id + '_value_0';
    var chooseByName = (name) => 'select[name=' + name + ']';

    var propertyFilterName = $(chooseByName(ruleFilter) + ' :selected').text();
    var arithmeticOperatorName = $(chooseByName(ruleFilterOperator)).val();
    var propertyFilterSecondName = $(chooseByName(ruleFilterSecond) + ' :selected').text();
    var comparisonOperatorName = $(chooseByName(ruleComparisonOperator)).val();
    var inputValue = $('input[name=' + ruleValue + ']').val();

//{ $eq: [ {$divide: [""$Population"", ""$Structure.Districsts""]}, 20] }
    return '{""$' + comparisonOperatorName
        + '"":[{""$' + arithmeticOperatorName
        + '"":[""$' + propertyFilterName
        + '"",""$' + propertyFilterSecondName
        + '""]},' + inputValue
        + ']}';
}";
            filter += @",
valueSetter: function(rule, value) {
    if (rule.operator.nb_inputs > 0) {
        var ruleFilter = rule.id + '_filter';
        var ruleOperator = rule.id + '_operator';
        var comparisonType = rule.id + '_comptype';
        var ruleFilterOperator = ruleFilter + '_operator';
        var ruleFilterSecond = ruleFilter + '_second';
        var ruleComparisonOperator = rule.id + '_operator_second';
        var ruleValue = rule.id + '_value_0';
        var chooseByName = (name) => 'select[name=' + name + ']';

        //   \{""$eq"":[{""$divide"":[""$FieldDec1"",""$FieldDec2""]},20]}
        //   \{""$1""\:\[\{""$2""\:\[""$3"",""$4""\]\},$5\]\}
        var values = value.match(
            /\{""\$([a-z]+)""\:\[\{""\$([a-z]+)""\:\[""\$([\w\.]+)"",""\$([\w\.]+)""\]\},(\d+|\d+\.?\d+)\]\}/
        );

        var comparisonOperatorName = values[1];
        var arithmeticOperatorName = values[2];
        var propertyFilterName = values[3];
        var propertyFilterSecondName = values[4];
        var inputValue = values[5];
        var onChangeValue = 'triggerChanges(\'' + rule.id + '\');';


        var filterOperatorHtml = $.parseHTML(
        '<select name=""' + ruleFilterOperator + '"" class=""form-select"" >'
            + '<option value=""add"">+</option>'
            + '<option value=""subtract"">-</option>'
            + '<option value=""multiply"">*</option>'
            + '<option value=""divide"">/</option>'
        + '</select>'
        );
        var ruleOperatorHtml = $.parseHTML(
        '<select name=""' + ruleComparisonOperator + '"" class=""form-select"" >'
            + '<option value=""eq"">equal</option>'
            + '<option value=""ne"">not equal</option>'
            + '<option value=""lt"">less</option>'
            + '<option value=""lte"">less or equal</option>'
            + '<option value=""gt"">greater</option>'
            + '<option value=""gte"">greater or equal</option>'
        + '</select>'
        );

        $(filterOperatorHtml).insertAfter(chooseByName(ruleFilter));
        $(chooseByName(ruleFilter)).clone(false)
            .attr('name', ruleFilterSecond)
            .insertAfter(chooseByName(ruleFilterOperator));
        $(ruleOperatorHtml).insertAfter(chooseByName(ruleFilterSecond));
        $('div#' + rule.id).children('div.rule-operator-container').attr('hidden', true);

        $(chooseByName(ruleFilterSecond)).val(propertyFilterSecondName + '_exp').change();
        $(chooseByName(ruleFilterOperator)).val(arithmeticOperatorName).change();
        $(chooseByName(ruleComparisonOperator)).val(comparisonOperatorName).change();
        rule.$el.find('.rule-value-container [name$=_0]').val(inputValue).trigger('change');

        $(chooseByName(ruleFilterSecond)).attr('onchange', onChangeValue);
        $(chooseByName(ruleFilterOperator)).attr('onchange', onChangeValue);
        $(chooseByName(ruleComparisonOperator)).attr('onchange', onChangeValue);

        // $(chooseByName(comparisonType)).val('_exp').change();
        // $(chooseByName(ruleFilter)).val(propertyFilterName + '_exp').change();
        // $(chooseByName(ruleOperator)).val('equal').change();
        // $('input[name=' + ruleValue + ']').val(inputValue).change();
    }
}";
            filter += ", \n operators: ['equal']";
            filter += ",\n default_operator: 'equal'";

            filter += "\n}";
            Filters.Add(filter);
            return true;
        }
    }

}