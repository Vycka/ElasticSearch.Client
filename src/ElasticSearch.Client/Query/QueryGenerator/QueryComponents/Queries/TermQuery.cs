using System.Dynamic;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Queries
{
    public class TermQuery : IQueryComponent
    {
        private readonly object _termQuery;

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

        public TermQuery(object termQuery)
        {
            _termQuery = termQuery;
        }

        private TermQuery(string key, object value)
        {
            var termQuery = new ExpandoObject();
            termQuery.Add(key, value);
            _termQuery = termQuery;
        }

        public object BuildQueryComponent()
        {
            object result = new
            {
                term = _termQuery
            };

            return result;
        }
    }
}