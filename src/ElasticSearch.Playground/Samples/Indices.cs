using System.Linq;
using ElasticSearch.Client;
using ElasticSearch.Client.ElasticSearch.Index;
using ElasticSearch.Client.ElasticSearch.Results;
using ElasticSearch.Client.Query.QueryGenerator;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Queries;
using ElasticSearch.Client.Utils;
using NUnit.Framework;

namespace ElasticSearch.Playground.Samples
{
    [TestFixture]
    public class Indices
    {
        [Test]
        public void IndiceMatchQuery()
        {
            var repSecIndex = new TimeStampedIndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            var repTempIndex = new TimeStampedIndexDescriptor("rep-templates-reader-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex, repTempIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Indices.AddIndices("rep-templates-reader-*");
            builder.Indices.SetQuery(new LuceneQuery("Level:\"INFO\""));
            
            builder.PrintQuery();

            ElasticSearchResult result = client.ExecuteQuery(builder);

            Assert.AreNotEqual(0, result.Items.Count);
            Assert.IsTrue(result.Items.All(i => i.Type == "rep-templates-reader"));
        }

        [Test]
        public void IndiceNotMatchQuery()
        {
            var repSecIndex = new TimeStampedIndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            var reptempIndex = new TimeStampedIndexDescriptor("rep-templates-reader-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex, reptempIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Indices.AddIndices("rep-templates-reader-*");
            builder.Indices.SetQuery(new LuceneQuery("type:\"rep-sec\""));
            builder.Indices.SetNoMatchQuery(new LuceneQuery("*"));

            builder.PrintQuery();

            ElasticSearchResult result = client.ExecuteQuery(builder);

            Assert.AreNotEqual(0, result.Items.Count);
            Assert.IsTrue(result.Items.All(i => i.Type == "rep-sec"));
        }
    }
}