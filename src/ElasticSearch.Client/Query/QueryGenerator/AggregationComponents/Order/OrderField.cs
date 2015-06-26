using System.Dynamic;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Sort;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Order
{
    public class OrderField : IAggregateOrder
    {
        private readonly string _field;
        private readonly SortOrder _sortOrder;

        public OrderField(string field = "_term", SortOrder sortOrder = SortOrder.Asc)
        {
            _field = field;
            _sortOrder = sortOrder;
        }

        public ExpandoObject BuildRequestComponent()
        {
            ExpandoObject orderObject = new ExpandoObject();

            orderObject.Add(_field, _sortOrder.ToString().ToLowerInvariant());

            return orderObject;
        }
    }
}