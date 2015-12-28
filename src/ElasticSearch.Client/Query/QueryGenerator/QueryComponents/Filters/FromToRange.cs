using System;
using System.Dynamic;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters
{
    public abstract class FromToRange 
    {
        protected readonly string FieldName;
        protected object FromValue;
        protected object ToValue;

        protected FromToRange(string fieldName)
        {
            if (fieldName == null)
                throw new ArgumentNullException("fieldName");

            FieldName = fieldName;
        }

        public ExpandoObject BuildRequestComponent()
        {
            var rangeFilter = new ExpandoObject();

            rangeFilter.Add(FieldName, new { from = FromValue, to = ToValue });

            ExpandoObject result = new ExpandoObject();
            result.Add("range",rangeFilter);

            return result;
        }

    }
}