using System;
using System.Collections.Generic;
using System.Dynamic;

namespace ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters
{
    public abstract class FromToRange 
    {
        protected readonly string FieldName;
        protected readonly object FieldFrom;
        protected readonly object FieldTo;

        public FromToRange(string fieldName, object fieldFrom, object fieldTo)
        {
            if (fieldName == null)
                throw new ArgumentNullException("fieldName");
            if (fieldFrom == null)
                throw new ArgumentNullException("fieldFrom");
            if (fieldTo == null)
                throw new ArgumentNullException("fieldTo");

            FieldName = fieldName;
            FieldFrom = fieldFrom;
            FieldTo = fieldTo;
        }

        public object BuildFilterComponent()
        {
            var rangeFilter = new ExpandoObject();

            ((IDictionary<string, object>)rangeFilter).Add(FieldName, new { from = FieldFrom, to = FieldTo });

            return new { range = rangeFilter };
        }

    }
}