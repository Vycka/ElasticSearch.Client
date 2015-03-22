namespace ElasticSearch.Client.ElasticSearch.Results
{
    public class ElasticSearchResult : SearchResult<ResultItem>
    {
        internal ElasticSearchResult(string searchResultJson)
            : base(searchResultJson)
        {
           
        }
    }
}