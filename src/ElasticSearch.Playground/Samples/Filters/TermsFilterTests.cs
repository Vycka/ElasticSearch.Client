using ElasticSearch.Client.ElasticSearch;
using ElasticSearch.Client.ElasticSearch.Results;
using ElasticSearch.Client.Query.QueryGenerator.Models;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters;
using ElasticSearch.Client.Utils;
using NUnit.Framework;

namespace ElasticSearch.Playground.Samples.Filters
{
    public class TermsFilterTests : TestBase
    {
        [Test]
        public void TermsFilter()
        {
            QueryBuilder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));
            QueryBuilder.Filtered.Filters.Add(FilterType.Must, 
                new TermsFilter("_type", "einstein_engine", "x")
                {
                    // Optional Params
                    Cache = false, 
                    Execution = "bool"
                }
            );

            QueryBuilder.PrintQuery();

            ElasticSearchResult result = Client.ExecuteQuery(QueryBuilder, new GetParam("search_type", "count"));

            Assert.GreaterOrEqual(result.Total, 1);
        } 
    }
}