using System;
using ElasticSearch.Client.ElasticSearch.Results;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Queries;
using ElasticSearch.Client.Utils;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ElasticSearch.Playground.Samples.Queries
{
    [TestFixture]
    public class SetQuery
    {
        [Test]
        public void ExpectDefaultCountReturned()
        {
            QueryBuilder builder = new QueryBuilder();
            builder.SetQuery(new TermQuery("_id","test"));

            string expected = "{\"query\":{\"term\":{\"_id\":\"test\"}}}";
            string actual = JsonConvert.SerializeObject(builder.BuildRequestObject());

            Assert.AreEqual(expected, actual);
        }
    }
}