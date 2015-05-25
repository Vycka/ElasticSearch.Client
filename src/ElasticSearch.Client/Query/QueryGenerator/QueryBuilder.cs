using System;
using System.Dynamic;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents;
using ElasticSearch.Client.Query.QueryGenerator.SectionBuilders;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator
{
    public class QueryBuilder
    {
        public int Size = 500;
        public IQueryComponent Query;

        public FilteredSectionBuilder Filtered = new FilteredSectionBuilder();
        public IndicesSectionBuilder Indices = new IndicesSectionBuilder();
        public SortListBuilder Sort = new SortListBuilder();
        // Aggregation is temporary, proper builder should be created
        public object Aggregation = null;

        public void SetQuery(IQueryComponent queryComponent)
        {
            Query = queryComponent;
        }
        public void SetSize(int size)
        {
            Size = size;
        }
        
        public object BuildRequestObject()
        {
            ExpandoObject requestObject = new ExpandoObject();

            ExpandoObject querySection = BuildQuerySection();

            if (querySection != null && Query != null)
                throw new InvalidOperationException("Simple QUERY must be alone, it can't work with INDICES or FILTERED");

            if (Query != null)
                requestObject.Add("query", Query.BuildQueryComponent());

            if (Aggregation != null)
                requestObject.Add("aggs", Aggregation);

            requestObject.AddIfNotNull("query", querySection);
            requestObject.Add("size",Size);
            requestObject.AddIfNotNull("sort", Sort.BuildSortSection());

            return requestObject;
        }

        internal ExpandoObject BuildQuerySection()
        {
            ExpandoObject querySection = new ExpandoObject();

            querySection.AddIfNotNull("indices", Indices.BuildIndicesSection());
            querySection.AddIfNotNull("filtered", Filtered.BuildFilteredSection());

            if (querySection.GetItems().Count != 0)
                return querySection;
            return null;
        }


    }




}