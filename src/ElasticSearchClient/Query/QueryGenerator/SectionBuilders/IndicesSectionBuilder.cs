using System.Collections.Generic;
using System.Dynamic;
using ElasticSearch.Query.QueryGenerator.QueryComponents;
using ElasticSearch.Utils;

namespace ElasticSearch.Query.QueryGenerator.SectionBuilders
{
    public class IndicesSectionBuilder
    {
        private readonly List<string> _indicesList = new List<string>();
        private IQueryComponent _query, _notQuery;

        internal IndicesSectionBuilder()
        {
        }

        public void AddIndices(params string[] indices)
        {
            _indicesList.AddRange(indices);
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
            if (_indicesList.Count == 0 && _query == null && _notQuery == null)
                return null;

            ExpandoObject indicesObject = new ExpandoObject();
            
            if (_indicesList.Count != 0)
                indicesObject.Add("indices",_indicesList);

            indicesObject.AddIfNotNull("query", (_query == null ? null : _query.BuildQueryComponent()));
            indicesObject.AddIfNotNull("no_match_query", (_notQuery == null ? null : _notQuery.BuildQueryComponent()));

            return indicesObject;
        }
    }
}