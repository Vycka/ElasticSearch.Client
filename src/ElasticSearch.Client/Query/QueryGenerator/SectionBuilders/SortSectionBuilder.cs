using System.Collections.Generic;
using System.Linq;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents;

namespace ElasticSearch.Client.Query.QueryGenerator.SectionBuilders
{
    public class SortListBuilder
    {
        public List<ISortComponent> Items = new List<ISortComponent>();

        internal SortListBuilder()
        {
        }

        public void Add(ISortComponent sortComponent)
        {
            Items.Add(sortComponent);
        }

        internal object BuildSortSection()
        {
            if (Items.Count == 0)
                return null;

            var sortObjects = Items.Select(qq => qq.BuildRequestComponent()).ToList();
            return sortObjects;
        }
    }
}