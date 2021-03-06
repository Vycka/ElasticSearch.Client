﻿using System;

namespace ElasticSearch.Client.Query.QueryGenerator.QueryComponents.Filters
{
    public class MovingTimeRange : FromToRange, IFilterComponent
    {
        private readonly int _lookupTimeSeconds;

        public MovingTimeRange(string fieldName, int lookupTimeSeconds)
            : base(fieldName)
        {
            FromValue = String.Format("now-{0}s", lookupTimeSeconds);
            ToValue = "now";

            _lookupTimeSeconds = lookupTimeSeconds;
        }


        public QueryDate GetQueryDate()
        {
            DateTime utcNow = DateTime.UtcNow;
            return new QueryDate(FieldName, utcNow.AddSeconds(-_lookupTimeSeconds), utcNow);
        }
    }
}