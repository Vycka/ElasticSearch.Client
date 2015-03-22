using System;
using ElasticSearch.Playground.Query.QueryGenerator.QueryComponents;

namespace ElasticSearch.Playground.Query.QueryGenerator.Models
{
    public class FilterItem
    {
        public readonly FilterType FilterType;
        public readonly IFilterComponent FilterComponent;

        public FilterItem(FilterType filterType, IFilterComponent filterComponent)
        {
            if (filterComponent == null)
                throw new ArgumentNullException("filterComponent");

            FilterType = filterType;
            FilterComponent = filterComponent;
        }
    }
}