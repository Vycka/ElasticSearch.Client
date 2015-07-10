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
            var client = new ElasticSearchClient("http://10.1.14.98:9200/", einsteinIndex);

            QueryBuilder query = new QueryBuilder();
            query.Filtered.Filters.Add(
                FilterType.Must,
                new FixedTimeRange(
                    "@timestamp",
                    DateTime.UtcNow.StartOfWeek(),
                    DateTime.UtcNow.StartOfWeek().EndOfDay()
                )
            );
            query.Filtered.Filters.Add(FilterType.Must, new LuceneFilter("EventType:GetReportDataVHandler AND _exists_:Request"));

            query.Aggregates.Add("cunt", new ValueCountAggregate("@timestamp"));

            var result = client.ExecuteQueryRaw(query,new GetParam("search_type","count"));

            Console.WriteLine(result);
        }
    }
}