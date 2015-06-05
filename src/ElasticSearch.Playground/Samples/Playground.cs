using System;
using ElasticSearch.Client;
using ElasticSearch.Client.ElasticSearch;
using ElasticSearch.Client.ElasticSearch.Index;
using ElasticSearch.Client.ElasticSearch.Results;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates;
using ElasticSearch.Client.Query.QueryGenerator.Models;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Queries;
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
            var repSecIndex = new TimeStampedIndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp", 86400));
            builder.Aggregates.Add("maxGeneration", new NestedAggregate(new TermsAggregate("CorrelationCode"),"max", new MaxAggregate("Event.TotalDuration")));

            builder.PrintQuery(client.IndexDescriptors);

            dynamic result = client.ExecuteAggregate(builder);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }

        [Test]
        [Ignore]
        public void CopyKibana4SettingsFromProdToDevs()
        {
            var kibana4Index = new ConcreteIndexDescriptor(".kibana");
            ElasticSearchClient client = new ElasticSearchClient("http://10.1.14.98:9200/", kibana4Index);
            QueryBuilder query = new QueryBuilder();

            query.SetQuery(new MatchAll());

            query.PrintQuery(client.IndexDescriptors);

            ElasticSearchResult result = client.ExecuteQuery(query);

            PrintIndexes(result);

            MigrateConfig("http://172.22.9.99:9200/", result);
            MigrateConfig("http://172.22.12.135:9200/", result);
            MigrateConfig("http://10.2.40.27:9200/", result);
        }

        private void MigrateConfig(string targetUrl, ElasticSearchResult queriedConfig)
        {
            HttpRequest httpRequest = new HttpRequest(targetUrl);

            foreach (ResultItem configItem in queriedConfig.Items)
            {
                AddOrReplaceConfigEntry(httpRequest, configItem);
            }
        }

        private void AddOrReplaceConfigEntry(HttpRequest httpRequest, ResultItem configEntry)
        {
            try
            {
                httpRequest.MakeRequest(configEntry.ToString(), RequestType.Delete);
            }
            catch (ExtendedWebException ex)
            {
                if (!ex.Message.Contains("404"))
                    throw;
            }

            httpRequest.MakePostJsonRequest(configEntry.ToString(), configEntry.Source.ToString());
        }

        private static void PrintIndexes(ElasticSearchResult result)
        {
            foreach (ResultItem index in result.Items)
            {
                Console.Out.WriteLine(index);
            }
        }

    }
}