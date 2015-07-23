using System.Dynamic;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters
{
    public class TermFilter : IFilterComponent
    {
        private readonly object _termFilter;

        public TermFilter(string key, int value)
            : this(key, (object)value)
        {
        }

        public TermFilter(string key, double value)
            : this(key, (object)value)
        {
        }

        public TermFilter(string key, string value)
            : this(key, (object)value)
        {
        }

        public TermFilter(string key, object value)
        {
            var termFilter = new ExpandoObject();
            termFilter.Add(key, value);
            _termFilter = termFilter;
        }

        public ExpandoObject BuildRequestComponent()
        {
            ExpandoObject result = new ExpandoObject();

            result.Add("term", _termFilter);

            return result;
        }

        public QueryDate GetQueryDate()
        {
            return null;
        }
    }
}