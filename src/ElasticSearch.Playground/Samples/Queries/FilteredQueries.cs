using System;
using System.Linq;
using ElasticSearch.Client;
using ElasticSearch.Client.ElasticSearch.Results;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Query.QueryGenerator.Models;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Queries;
using ElasticSearch.Client.Utils;
using NUnit.Framework;

namespace ElasticSearch.Playground.Samples.Queries
{
    [TestFixture]
    public class FilteredQueries : TestBase
    {
        [Test]
        public void ExecuteFilteredQuery()
        {

            QueryBuilder builder = new QueryBuilder();
            builder.Filtered.Queries.Add(QueryType.Must, new TermQuery("_type","einstein_engine"));
            builder.Filtered.Filters.Add(FilterType.Must, new MovingTimeRange("@timestamp",86400));
            
            builder.PrintQuery();
            
            ElasticSearchResult result = Client.ExecuteQuery(builder);

            Assert.AreNotEqual(0, result.Items.Count);
            Assert.IsTrue(result.Items.All(i => i.Type == "einstein_engine"));
            Assert.IsTrue(result.Items.All(i => DateTime.Parse((i.Source["@timestamp"].ToString())) > DateTime.UtcNow.AddDays(-1)));
        }
    }
}