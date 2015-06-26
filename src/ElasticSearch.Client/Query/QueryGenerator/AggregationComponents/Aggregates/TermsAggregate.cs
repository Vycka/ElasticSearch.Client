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

            SetComponentProperty("field", field);
            Size = size;

        }

        public IAggregateOrder Order
        {
            get { return (IAggregateOrder)GetComponentProperty("order"); }
            set { SetComponentProperty("order", value); }
        }
    }
}