using System;

namespace ElasticSearch.Playground.Query.QueryGenerator.QueryComponents.Queries
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

        public object BuildQueryComponent()
        {
            return new {  query_string = new { query = _queryString } };
        }
    }
}