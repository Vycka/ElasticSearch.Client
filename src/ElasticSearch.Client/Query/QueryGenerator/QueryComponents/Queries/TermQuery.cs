using System;
using System.Collections.Generic;
using System.Dynamic;

namespace ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Queries
{
    public class TermQuery : IQueryComponent
    {
        private readonly string _key;
        private readonly object _value;

        public TermQuery(string key, int value)
            : this(key, (object)value)
        {
        }

        public TermQuery(string key, int[] value)
            : this(key, (object)value)
        {
        }

        public TermQuery(string key, double value)
            : this(key, (object)value)
        {
        }

        public TermQuery(string key, double[] value)
            : this(key, (object)value)
        {
        }

        public TermQuery(string key, string value)
            : this(key, (object)value)
        {
        }

        public TermQuery(string key, string[] value)
            : this(key, (object)value)
        {
        }

        public TermQuery(string key, object[] value)
            : this(key, (object)value)
        {
        }

        private TermQuery(string key, object value)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            _key = key;
            _value = value;
        }

        public object BuildQueryComponent()
        {
            ExpandoObject termKeyValue = new ExpandoObject();
            ((IDictionary<string, object>)termKeyValue).Add(_key, _value);

            object result = new
            {
                term = termKeyValue
            };

            return result;
        }
    }
}