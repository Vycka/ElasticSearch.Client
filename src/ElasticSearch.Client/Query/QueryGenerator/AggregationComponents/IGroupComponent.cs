using System.Security.Cryptography.X509Certificates;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents
{
    public interface IGroupComponent
    {

        string OperationName { get; }
        object BuildRequestComponent();
    }
}