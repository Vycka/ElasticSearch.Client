using ElasticSearch.Playground.ElasticSearch;
using ElasticSearch.Playground.ElasticSearch.Index;
using ElasticSearch.Playground.Query.QueryGenerator;

namespace ElasticSearch.Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            ElasticSearchIndexDescriptor repSecIndex = new TimeStampedIndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();

            builder.BuildRequestObject();

            ElasticSearchResult result = client.ExecuteQuery(builder);
        }
    }
}
