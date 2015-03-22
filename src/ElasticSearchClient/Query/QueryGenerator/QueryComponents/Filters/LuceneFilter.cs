using System;

namespace ElasticSearchClient.Query.QueryGenerator.QueryComponents.Filters
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

        public object BuildFilterComponent()
        {
            return new { fquery = new { query = new { query_string = new { query = _queryString } }, _cache = true } };
        }

        public QueryDate GetQueryDate()
        {
            return null;
        }
    }
}