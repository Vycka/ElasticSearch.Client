namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents
{
    public interface IAggregateComponent
    {
        string OperationName { get; }
        object BuildRequestComponent();
    }
}