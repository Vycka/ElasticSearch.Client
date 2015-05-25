using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using ElasticSearch.Client.Query.QueryGenerator.Models;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.SectionBuilders
{
    public class FilterListBuilder
    {
        internal FilterListBuilder()
        {
        }

        public List<FilterItem> Items = new List<FilterItem>();

        public void Add(FilterType filterType, IFilterComponent filterComponent)
        {
            Items.Add(new FilterItem(filterType, filterComponent));
        }

        public void Add(FilterItem filterItem)
        {
            Items.Add(filterItem);
        }

        internal ExpandoObject BuildFilterBoolRequest()
        {
            ExpandoObject requestObject = new ExpandoObject();
            foreach (FilterType filterType in Enum.GetValues(typeof(FilterType)))
            {
                var localFilterType = filterType;
                var filterObjects = Items
                    .Where(q => q.FilterType == localFilterType)
                    .Select(qq => qq.FilterComponent.BuildRequestComponent()).ToList();

                if (filterObjects.Count != 0)
                    requestObject.Add(FilterTypeMapping.GetName(filterType), filterObjects);
            }
            if (requestObject.GetItems().Count != 0)
                return requestObject;

            return null;
        }
    }
}