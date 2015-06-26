using System;
using System.Dynamic;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters
{
    public class LuceneFilter : IFilterComponent
    {
        private readonly string _queryString;

        public LuceneFilter(string queryString)
        {
            if (queryString == null)
                throw new ArgumentNullException("queryString");

            _queryString = queryString;
        }

        public ExpandoObject BuildRequestComponent()
        {
            ExpandoObject result = new ExpandoObject();
            result.Add("fquery", new {query = new {query_string = new {query = _queryString}}});
            result.Add("_cache", true);

            return result;
        }

        public QueryDate GetQueryDate()
        {
            return null;
        }
    }
}