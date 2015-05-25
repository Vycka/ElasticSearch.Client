namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents
{
    public interface IAggregateComponent
    {
        object BuildRequestComponent();
    }
}