using System;
using System.Dynamic;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents;
using ElasticSearch.Client.Utils;

namespace MyCool.Namespace
{
    // By implementing IFilterComponent, you can create your own query features, which are not present in this library.
    // also if they represent - whats officially supported in ES
    public class MyCustomLuceneFilter : IFilterComponent
    {
        public bool Cache = true;

        private readonly string _queryString;

        public MyCustomLuceneFilter(string queryString)
        {
            if (queryString == null)
                throw new ArgumentNullException("queryString");

            _queryString = queryString;
        }

        public ExpandoObject BuildRequestComponent()
        {
            // You can add stuff into this object, and it will appear in the query it self.
            object queryObject = new
            {
                query = new
                {
                    query_string = new
                    {
                        query = _queryString,

                        // Based on this: https://www.elastic.co/guide/en/elasticsearch/reference/current/query-dsl-query-string-query.html
                        // My best guess is that additional parameters should go here. like the analyze_wildcard for this example

                        analyze_wildcard = true,
                    },
                },
                _cache = Cache,
            };

            ExpandoObject result = new ExpandoObject();
            result.Add("fquery", queryObject);

            return result;
        }

        public QueryDate GetQueryDate()
        {
            return null;
        }
    }
}