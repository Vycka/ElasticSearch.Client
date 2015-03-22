using System.Diagnostics;
using System.Linq;
using ElasticSearchClient.ElasticSearch;
using ElasticSearchClient.Playground.Utils;
using ElasticSearchClient.Query.QueryGenerator;
using ElasticSearchClient.Query.QueryGenerator.QueryComponents.Queries;
using NUnit.Framework;

namespace ElasticSearchClient.Playground.Samples
{
    [TestFixture]
    public class Indices
    {
        [Test]
        public void IndiceMatchQuery()
        {
            IndexDescriptor repSecIndex = new IndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();
            builder.Indices.AddIndices("rep-templates-reader-*");
            builder.Indices.SetQuery(new LuceneQuery("Level:\"INFO\""));

            builder.PrintQuery();

            ElasticSearchResult result = client.ExecuteQuery(builder);

            Assert.AreNotEqual(0, result.Items.Count);
            Assert.IsTrue(result.Items.All(i => i.Type == "rep-sec"));
        }

        [Test]
        public void IndiceNotMatchQuery()
        {
            IndexDescriptor repSecIndex = new IndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex);

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