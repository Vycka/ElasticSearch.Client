using ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Order;

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

            Components.Set("field", field);
            Size = size;

        }

        public IAggregateOrder Order
        {
            get { return Components.Get<IAggregateOrder>("order"); }
            set { Components.Set("order", value); }
        }
    }
}