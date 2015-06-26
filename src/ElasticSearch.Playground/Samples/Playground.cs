using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ElasticSearch.Client;
using ElasticSearch.Client.ElasticSearch.Index;
using ElasticSearch.Client.ElasticSearch.Results;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates;
using ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Order;
using ElasticSearch.Client.Query.QueryGenerator.Models;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Queries;
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
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp",864000));

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
            var repSecIndex = new TimeStampedIndexDescriptor("einstein_engine-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://10.1.14.98:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 3600));
            builder.Filtered.Filters.Add(FilterType.Must, new TermFilter("CorrelationCode", "an_156498BA-045A-42F2-B540-7D1B6F1692AF"));

            var result = client.ExecuteQuery(builder);
        }
    }
}