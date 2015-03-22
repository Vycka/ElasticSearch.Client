using System;
using System.Linq;
using ElasticSearch.Client;
using ElasticSearch.Client.ElasticSearch;
using ElasticSearch.Client.ElasticSearch.Index;
using ElasticSearch.Client.ElasticSearch.Results;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Query.QueryGenerator.Models;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Queries;
using NUnit.Framework;

namespace ElasticSearch.Playground.Samples
{
    [TestFixture]
    public class FilteredQueries
    {

        [Test]
        public void ExecuteFiltereQuery()
        {
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/");

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Queries.Add(QueryType.Must, new TermQuery("_type","rep-sec"));
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp",86400));
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));
            ElasticSearchResult result = client.ExecuteQuery(builder);

            Assert.AreNotEqual(0, result.Items.Count);
            Assert.IsTrue(result.Items.All(i => i.Type == "rep-sec"));
            Assert.IsTrue(result.Items.All(i => DateTime.Parse((i.Source["@timestamp"].ToString())) > DateTime.UtcNow.AddDays(-1)));
        }

        [Ignore]
        [Test]
        public void Temp()
        {
              var rabbitIndex = new TimeStampedIndexDescriptor("RabbitMQ-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
              var securityIndex = new TimeStampedIndexDescriptor("Security-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
              ElasticSearchClient client = new ElasticSearchClient("http://localhost:9200/", rabbitIndex, securityIndex);

              QueryBuilder builder = new QueryBuilder();
              builder.Filtered.Queries.Add(QueryType.Should, new TermQuery("Level","ERROR"));

              builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));
              builder.Filtered.Filters.Add(FilterType.MustNot, new LuceneFilter("EventType:IrrelevantError"));

              ElasticSearchResult result = client.ExecuteQuery(builder);
        }
    }
}