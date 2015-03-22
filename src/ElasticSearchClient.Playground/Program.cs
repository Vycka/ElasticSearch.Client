using ElasticSearchClient.ElasticSearch;
using ElasticSearchClient.Query.QueryGenerator;

namespace ElasticSearchClient.Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            IndexDescriptor repSecIndex = new IndexDescriptor("rep-sec-", "yyyy.MM.dd", "@timestamp", IndexStep.Day);
            ElasticSearchClient client = new ElasticSearchClient("http://172.22.1.31:9200/", repSecIndex);

            QueryBuilder builder = new QueryBuilder();

            builder.BuildRequestObject();

            ElasticSearchResult result = client.ExecuteQuery(builder);
        }
    }
}
