using System.Dynamic;
using ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Order;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class TermsAggregate : AggregateComponentBase, IGroupComponent
    {
        public TermsAggregate()
            : this(null)
        {
        }

        public TermsAggregate(string field, int? size = null) 
            : base("terms")
        {
            ExpandoObject termsRequest = new ExpandoObject();

            termsRequest.AddIfNotNull("field", field);
            termsRequest.AddIfNotNull("size", size);

            SetOperationObject(termsRequest);
        }

        public IAggregateOrder Order
        {
            get { return (IAggregateOrder)GetFromOperationObject("order"); }
            set { UpdateOperationObject("order", value); }
        }
    }
}