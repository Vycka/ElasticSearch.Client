using System;
using System.Collections.Generic;
using System.Linq;
using ElasticSearchClient.ElasticSearch;
using ElasticSearchClient.Query.QueryGenerator;
using ElasticSearchClient.Query.QueryGenerator.Models;
using ElasticSearchClient.Query.QueryGenerator.QueryComponents.Filters;
using ElasticSearchClient.Query.QueryGenerator.QueryComponents.Queries;
using NUnit.Framework;

namespace ElasticSearchClient.Playground.Samples
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

            ElasticSearchResult result = client.ExecuteQuery(builder);

            Assert.AreNotEqual(0, result.Items.Count);
            Assert.IsTrue(result.Items.All(i => i.Type == "rep-sec"));
            Assert.IsTrue(result.Items.All(i => DateTime.Parse((i.Source["@timestamp"].ToString())) > DateTime.UtcNow.AddDays(-1)));
        }
    }
}