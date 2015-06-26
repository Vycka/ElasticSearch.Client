using System;
using System.Dynamic;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Queries
{
    public class LuceneQuery : IQueryComponent
    {
        private readonly string _queryString;

        public LuceneQuery(string queryString)
        {
            if (queryString == null)
                throw new ArgumentNullException("queryString");

            _queryString = queryString;
        }

        public ExpandoObject BuildRequestComponent()
        {
            ExpandoObject result = new ExpandoObject();

            result.Add("query_string", new { query = _queryString });

            return result;
        }
    }
}