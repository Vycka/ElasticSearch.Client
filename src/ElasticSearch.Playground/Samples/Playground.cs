using ElasticSearch.Client;
using ElasticSearch.Client.ElasticSearch.Index;
using ElasticSearch.Client.ElasticSearch.Results;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Query.QueryGenerator.Models;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Queries;
using ElasticSearch.Playground.Utils;
using NUnit.Framework;

namespace ElasticSearch.Playground.Samples
{

    [TestFixture]
    public class Playground
    {

        [Test]
        [Ignore]
        public void Test()
        {
            var repSecIndex = new TimeStampedIndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            var repTempIndex = new TimeStampedIndexDescriptor("rep-templates-reader-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://10.0.22.16:9200/", repSecIndex, repTempIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Queries.Add(QueryType.Should, new MatchAll());
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp",864000));
            builder.Filtered.Filters.Add(FilterType.Must, new TermFilter("Event.Exception.ClassName", "System.InvalidOperationException"));

            builder.PrintQuery();

            ElasticSearchResult result = client.ExecuteQuery(builder);

            Assert.AreNotEqual(0, result.Items.Count);
        }
    }
}