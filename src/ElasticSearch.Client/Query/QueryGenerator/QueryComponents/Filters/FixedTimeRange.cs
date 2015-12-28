using System;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters
{
    public class FixedTimeRange : FromToRange, IFilterComponent
    {
        private DateTime _utcFrom, _utcTo;

        public DateTime UtcFrom
        {
            set
            {
                _utcFrom = value;
                FromValue = value.ToUnixTimeMs();
            }
            get
            {
                return _utcFrom;
            }
        }

        public DateTime UtcTo
        {
            set
            {
                _utcTo = value;
                ToValue = value.ToUnixTimeMs();
            }
            get
            {
                return _utcTo;
            }
        }

        public FixedTimeRange(string fieldName) : base(fieldName)
        {
            UtcFrom = DateTimeUnixTimeExtensions.UnixTimeStart;
            UtcTo = DateTimeUnixTimeExtensions.UnixTimeStart;
        }

        public FixedTimeRange(string fieldName, DateTime utcFrom, DateTime utcTo)
            : base(fieldName)
        {
            UtcFrom = utcFrom;
            UtcTo = utcTo;
        }


        public QueryDate GetQueryDate()
        {
            return new QueryDate(FieldName, _utcFrom, _utcTo);
        }
    }
}