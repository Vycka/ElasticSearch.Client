using System;
using System.Dynamic;
using System.Linq;
using ElasticSearch.Client.Utils;
using Newtonsoft.Json.Schema;

namespace ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters
{
    public class TermFilter : IFilterComponent
    {
        private readonly object _termFilter;

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

        public TermFilter(object termFilter)
        {
            _termFilter = termFilter;
        }

        private TermFilter(string key, object value)
        {
            var termFilter = new ExpandoObject();
            termFilter.Add(key, value);
            _termFilter = termFilter;
        }

        public object BuildRequestComponent()
        {
            object result = new
            {
                term = _termFilter
            };

            return result;
        }

        public QueryDate GetQueryDate()
        {
            return null;
        }
    }
}