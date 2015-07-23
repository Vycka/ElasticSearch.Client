using System;
using ElasticSearch.Client;
using ElasticSearch.Client.ElasticSearch;
using ElasticSearch.Client.ElasticSearch.Index;
using ElasticSearch.Client.ElasticSearch.Results;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates;
using ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Order;
using ElasticSearch.Client.Query.QueryGenerator.Models;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Sort;
using ElasticSearch.Client.Utils;
using Newtonsoft.Json;
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
            builder.Filtered.Filters.Add(FilterType.Must, new LuceneFilter("chujeris"));
            builder.Aggregates.Add("2", new TermsAggregate("key", 9999) { Order = new OrderField("_count", SortOrder.Desc)});

            builder.PrintQuery(client.IndexDescriptors);

            ElasticSearchResult result = client.ExecuteQuery(builder);

            Assert.AreNotEqual(0, result.Items.Count);
        }


        [Test]
        [Ignore]
        public void AggregateTest()
        {
            var repSecIndex = new TimeStampedIndexDescriptor("reporting_analytics_ui-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.9.99:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new FixedTimeRange("@timestamp", DateTime.Now.Yesterday(), DateTime.Now.EndOfDay()));
            builder.Size = 0;

            builder.Aggregates.Add(
                "template_aggregate",
                new TermsAggregate("TemplateId") { Script  = "TemplateName" }
            );

            builder.PrintQuery(client.IndexDescriptors);

            dynamic result = client.ExecuteQuery(builder);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }

        [Test]
        [Ignore]
        public void Xxx()
        {
            var einsteinIndex = new TimeStampedIndexDescriptor("einstein_engine-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            var client = new ElasticSearchClient("http://172.22.9.99:9200/", einsteinIndex);

            QueryBuilder qB = new QueryBuilder();
            qB.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp",86400));
            qB.Filtered.Filters.Add(FilterType.Must, new TermsFilter("CorrelationCode",  "tl_DD8AC340-B0ED-4C52-B2A3-38ECF6F2CDF5", "3b3b49de-72cc-499e-bf8e-f8aa6bd4c9cb", "ITALY") { Execution = "bool"});

            qB.PrintQuery();

            ElasticSearchResult result = client.ExecuteQuery(qB, new GetParam("search_type","count"));

            Console.WriteLine(result.Total);
        }
    }
}