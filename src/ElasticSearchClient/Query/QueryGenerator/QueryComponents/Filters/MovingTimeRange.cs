using System;

namespace ElasticSearch.Query.QueryGenerator.QueryComponents.Filters
{
    public class MovingTimeRange : FromToRange, IFilterComponent
    {
        private readonly int _lookupTimeSeconds;

        public MovingTimeRange(string fieldName, int lookupTimeSeconds)
            : base(fieldName, String.Format("now-{0}s", lookupTimeSeconds), "now")
        {
            _lookupTimeSeconds = lookupTimeSeconds;
        }

        public QueryDate GetQueryDate()
        {
            DateTime utcNow = DateTime.UtcNow;
            return new QueryDate(FieldName, utcNow.AddSeconds(-_lookupTimeSeconds), utcNow);
        }
    }
}