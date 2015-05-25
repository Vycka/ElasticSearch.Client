using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using ElasticSearch.Client.Query.QueryGenerator.QueryComponents;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.SectionBuilders
{
    public class IndicesSectionBuilder
    {
        public readonly List<string> IndcesItems = new List<string>();
        private IQueryComponent _query, _notQuery;

        internal IndicesSectionBuilder()
        {
        }

        public void AddIndices(params string[] indices)
        {
            IndcesItems.AddRange(indices);
        }

        public void SetQuery(IQueryComponent queryComponent)
        {
            _query = queryComponent;
        }

        public void SetNoMatchQuery(IQueryComponent queryComponent)
        {
            _notQuery = queryComponent;
        }

        internal ExpandoObject BuildIndicesSection()
        {
            if (IndcesItems.Count == 0 && _query == null && _notQuery == null)
                return null;

            if (IndcesItems.Count > 0 && _query == null) 
                Debug.WriteLine("WARNING: Query might fail: If at least one Indice is provided, MatchingQuery must be defined too!");

            ExpandoObject indicesObject = new ExpandoObject();

            if (IndcesItems.Count != 0)
                indicesObject.Add("indices", IndcesItems);

            indicesObject.AddIfNotNull("query", (_query == null ? null : _query.BuildRequestComponent()));
            indicesObject.AddIfNotNull("no_match_query", (_notQuery == null ? null : _notQuery.BuildRequestComponent()));

            return indicesObject;
        }
    }
}