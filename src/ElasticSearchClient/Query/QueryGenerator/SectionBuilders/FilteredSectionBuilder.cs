using System.Dynamic;
using ElasticSearchClient.Utils;

namespace ElasticSearchClient.Query.QueryGenerator.SectionBuilders
{
    public class FilteredSectionBuilder
    {
        public QueryListBuilder Queries = new QueryListBuilder();
        public FilterListBuilder Filters = new FilterListBuilder();

        internal object BuildFilteredSection()
        {
            ExpandoObject filteredSection = new ExpandoObject();

            if (Queries.Items.Count != 0)
                filteredSection.Add("query", new { @bool = Queries.BuildQueryBoolRequest() });

            if (Filters.Items.Count != 0)
                filteredSection.Add("filter", new { @bool = Filters.BuildFilterBoolRequest() });

            if (filteredSection.GetItems().Count != 0)
                return filteredSection;
            return null;
        }
    }
}