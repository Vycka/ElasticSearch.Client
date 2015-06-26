using System.Dynamic;

namespace ElasticSearch.Client.Query.QueryGenerator
{
    public interface IRequestComponent
    {
        ExpandoObject BuildRequestComponent();
    }
}