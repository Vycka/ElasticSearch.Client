namespace ElasticSearch.Client.Query.QueryGenerator
{
    public interface IRequestComponent
    {
        object BuildRequestComponent();
    }
}