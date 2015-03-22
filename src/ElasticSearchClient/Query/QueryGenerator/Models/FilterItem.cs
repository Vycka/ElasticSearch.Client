using System;
using ElasticSearch.Query.QueryGenerator.QueryComponents;

namespace ElasticSearch.Query.QueryGenerator.Models
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