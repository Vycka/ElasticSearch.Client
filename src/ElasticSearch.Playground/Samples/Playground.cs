﻿using System;
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
        public void TimeZoneErrors()
        {
            var repSecIndex = new TimeStampedIndexDescriptor("einstein_engine-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://10.1.14.98:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));
            builder.Filtered.Filters.Add(FilterType.Must, new LuceneFilter("Exception.Message:\"Can not get timezone offset. Time zone name is invalid.\" AND _exists_:CurrentUserId"));
            var termAggregate = new TermsAggregate("CurrentUserId");
            termAggregate.Order = new OrderField();
            builder.Aggregates.Add("terms", termAggregate);

            builder.PrintQuery(client.IndexDescriptors);

            AggregateResult result = client.ExecuteAggregate(builder);
            dynamic[] values = result.GetValues("terms.buckets");
            foreach (dynamic value in values)
            {
                Console.WriteLine(value.key + "  " + value.doc_count);
            }
        }

        [Test]
        [Ignore]
        public void Aaa()
        {
            var reportingAnalyticsIndex = new TimeStampedIndexDescriptor("reporting_analytics_ui-", "yyyy.MM.*", "@timestamp", IndexStep.Month);
            var client = new ElasticSearchClient("http://10.1.14.98:9200/", reportingAnalyticsIndex);

            var builder = new QueryBuilder { Size = 1000 };
            builder.SetQuery(new LuceneQuery("Level: Error OR LogType: Error"));

            ElasticSearchResult result = client.ExecuteQuery(builder);
            var x = result.Items[0].Source.Message;
        }

        public class MessageItem
        {
            public string Message { get; set; }
        }
    }
}