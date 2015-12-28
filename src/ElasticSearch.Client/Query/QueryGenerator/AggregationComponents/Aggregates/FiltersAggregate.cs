using System.Dynamic;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents;
using ElasticSearch.Client.Query.Utils;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class FiltersAggregate : IGroupComponent
    {
        protected readonly ComponentBag Components = new ComponentBag();

        public ExpandoObject BuildRequestComponent()
        {
            if (Components.Count == 0)
                return null;

            ComponentBag filtersComponent = new ComponentBag();
            filtersComponent.Set("filters", Components);

            ExpandoObject requestComponent = new ExpandoObject();
            requestComponent.Add("filters", filtersComponent);

            return requestComponent;
        }

        public FiltersAggregate Add(string name, IFilterComponent filter)
        {
            Components.Set(name, filter);

            return this;
        }
    }
}