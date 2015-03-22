using System;
using System.Collections.Generic;
using System.Dynamic;

namespace ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters
{
    public class TermFilter : IFilterComponent
    {
        private readonly string _key;
        private readonly object _value;

        public TermFilter(string key, int value)
            : this(key, (object)value)
        {
        }

        public TermFilter(string key, int[] value)
            : this(key, (object)value)
        {
        }

        public TermFilter(string key, double value)
            : this(key, (object)value)
        {
        }

        public TermFilter(string key, double[] value)
            : this(key, (object)value)
        {
        }

        public TermFilter(string key, string value)
            : this(key, (object)value)
        {
        }

        public TermFilter(string key, string[] value)
            : this(key, (object)value)
        {
        }

        public TermFilter(string key, object[] value)
            : this(key, (object)value)
        {
        }

        private TermFilter(string key, object value)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            _key = key;
            _value = value;
        }

        public object BuildFilterComponent()
        {
            ExpandoObject termKeyValue = new ExpandoObject();
            ((IDictionary<string, object>)termKeyValue).Add(_key, _value);

            object result = new
            {
                term = termKeyValue
            };

            return result;
        }

        public QueryDate GetQueryDate()
        {
            return null;
        }
    }
}