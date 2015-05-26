using System.Dynamic;
using ElasticSearch.Client.Query.QueryGenerator.Models;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class TermsAggregate : AggregateComponentBase
    {
        public TermsAggregate(string aggregateField, int? size = null)
        {
            ExpandoObject termRequest = new ExpandoObject();
            termRequest.Add("field", aggregateField);
            termRequest.AddIfNotNull("size", size);

            Add(AggregateType.Terms.GetName(), termRequest);
        }
    }
}