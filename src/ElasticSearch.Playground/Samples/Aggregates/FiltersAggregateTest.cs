using ElasticSearch.Client.ElasticSearch.Results;
using ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters;
using ElasticSearch.Client.Utils;
using NUnit.Framework;

namespace ElasticSearch.Playground.Samples.Aggregates
{
    [TestFixture]
    public class FiltersAggregateTest : TestBase
    {
        [Test]
        public void FilterByLucene()
        {
            var filters = new FiltersAggregate();
            filters.Add("Success", new LuceneFilter("Level:Info"));
            filters.Add("Error", new LuceneFilter("Level:Error"));

            //var filtersSubAggregate = new SubAggregate(filters, "counts", new CountAggregate("@timestamp"));

            QueryBuilder.Aggregates.Add("requests", filters);

            QueryBuilder.PrintQuery();

            AggregateResult result = Client.ExecuteAggregate(QueryBuilder);
            result.PrintResult();
        }
    }
}