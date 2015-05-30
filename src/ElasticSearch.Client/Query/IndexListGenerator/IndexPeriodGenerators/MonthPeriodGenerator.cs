using System;
using System.Collections.Generic;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.IndexListGenerator.IndexPeriodGenerators
{
    internal static class MonthPeriodGenerator
    {
        public static IEnumerable<DateTime> Generate(DateTime timeFrom, DateTime timeTo)
        {
            for (DateTime currentTime = timeFrom.StartOfMonth(); currentTime <= timeTo; currentTime = currentTime.AddMonths(1))
            {
                yield return currentTime;
            }
        }
    }
}