namespace ElasticSearch.Client.ElasticSearch.Results
{
    public class ElasticSearchResult : SearchResult<ResultItem>
    {
        internal ElasticSearchResult(dynamic searchResult)
            : base((object)searchResult)
        {
           
        }
    }
}