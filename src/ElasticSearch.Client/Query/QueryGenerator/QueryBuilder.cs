using System;
using System.Dynamic;
using ElasticSearch.Client.Query.QueryGenerator.AggregationComponents;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents;
using ElasticSearch.Client.Query.QueryGenerator.SectionBuilders;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator
{
    public class QueryBuilder
    {
        public int? Size = null;
        public IQueryComponent Query;

        public readonly FilteredSectionBuilder Filtered = new FilteredSectionBuilder();
        public readonly IndicesSectionBuilder Indices = new IndicesSectionBuilder();
        public readonly SortListBuilder Sort = new SortListBuilder();
        // Aggregation is temporary, proper builder should be created
        public readonly AggregationQuery Aggregates = new AggregationQuery();

        public void SetQuery(IQueryComponent queryComponent)
        {
            Query = queryComponent;
        }
        
        // TODO: Enough of those ifs, create IRequestComponent or smth
        public object BuildRequestObject()
        {
            ExpandoObject requestObject = new ExpandoObject();

            ExpandoObject querySection = BuildQuerySection();

            if (querySection != null && Query != null)
                throw new InvalidOperationException("Simple QUERY must be alone, it can't work with INDICES or FILTERED");

            if (Query != null)
                requestObject.Add("query", Query.BuildRequestComponent());


            requestObject.AddIfNotNull("query", querySection);
            requestObject.AddIfNotNull("aggs", Aggregates.BuildRequestComponent());
            requestObject.AddIfNotNull("size", Size);
            requestObject.AddIfNotNull("sort", Sort.BuildSortSection());

            return requestObject;
        }

        private ExpandoObject BuildQuerySection()
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