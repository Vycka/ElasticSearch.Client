using ElasticSearch.Client.ElasticSearch;
using ElasticSearch.Client.ElasticSearch.Results;
using ElasticSearch.Client.Query.QueryGenerator.Models;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Queries;
using ElasticSearch.Client.Utils;
using NUnit.Framework;

namespace ElasticSearch.Playground.Samples.Queries
{
    [TestFixture]
    public class TermsQueryTests : TestBase
    {
        [Test]
        public void TermsQuery()
        {
            QueryBuilder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp",86400));
            QueryBuilder.Filtered.Queries.Add(QueryType.Must, new TermsQuery("_type", "einstein_engine", "x") { MinimumShouldMatch = 1 });

            QueryBuilder.PrintQuery();

            ElasticSearchResult result = Client.ExecuteQuery(QueryBuilder, new GetParam("search_type", "count"));

            Assert.GreaterOrEqual(result.Total, 1);
        }
    }
}