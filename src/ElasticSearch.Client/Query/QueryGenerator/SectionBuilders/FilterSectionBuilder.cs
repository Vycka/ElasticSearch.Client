using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using ElasticSearch.Playground.Query.QueryGenerator.Models;
using ElasticSearch.Playground.Query.QueryGenerator.QueryComponents;
using ElasticSearch.Playground.Utils;

namespace ElasticSearch.Playground.Query.QueryGenerator.SectionBuilders
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
                var filterObjects = Items
                    .Where(q => q.FilterType == filterType)
                    .Select(qq => qq.FilterComponent.BuildFilterComponent()).ToList();

                if (filterObjects.Count != 0)
                    requestObject.Add(FilterTypeMapping.GetName(filterType), filterObjects);
            }
            if (requestObject.GetItems().Count != 0)
                return requestObject;

            return null;
        }
    }
}