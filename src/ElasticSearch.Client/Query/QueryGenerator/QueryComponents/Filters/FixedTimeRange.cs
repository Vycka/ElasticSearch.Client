using System;
using ElasticSearch.Playground.Utils;

namespace ElasticSearch.Playground.Query.QueryGenerator.QueryComponents.Filters
{
    public class FixedTimeRange : FromToRange, IFilterComponent
    {
        private readonly DateTime _utcFrom, _utcTo;

        public FixedTimeRange(string fieldName, DateTime utcFrom, DateTime utcTo)
            : base(fieldName, utcFrom.ToUnixTimeMs(), utcTo.ToUnixTimeMs())
        {
            _utcFrom = utcFrom;
            _utcTo = utcTo;
        }


        public QueryDate GetQueryDate()
        {
            return new QueryDate(FieldName, _utcFrom, _utcTo);
        }
    }
}