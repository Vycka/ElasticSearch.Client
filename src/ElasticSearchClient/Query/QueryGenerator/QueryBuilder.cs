using System.Dynamic;
using ElasticSearchClient.Query.QueryGenerator.SectionBuilders;
using ElasticSearchClient.Utils;

namespace ElasticSearchClient.Query.QueryGenerator
{
    public class QueryBuilder
    {
        

        public int Size = 500;

        public FilteredSectionBuilder Filtered = new FilteredSectionBuilder();
        public IndicesSectionBuilder Indices = new IndicesSectionBuilder();
        public SortListBuilder Sort = new SortListBuilder();

        public void SetSize(int size)
        {
            Size = size;
        }
        
        public object BuildRequestObject()
        {
            ExpandoObject requestObject = new ExpandoObject();

            requestObject.AddIfNotNull("query", BuildQuerySection());
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