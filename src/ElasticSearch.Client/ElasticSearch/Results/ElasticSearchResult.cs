namespace ElasticSearch.Client.ElasticSearch.Results
{
    public class ElasticSearchResult : SearchResult<dynamic>
    {
        internal ElasticSearchResult(dynamic searchResult)
            : base((object)searchResult)
        {
           
        }
    }
}