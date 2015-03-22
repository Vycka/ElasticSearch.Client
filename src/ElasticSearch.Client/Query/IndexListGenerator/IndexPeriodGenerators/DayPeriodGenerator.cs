using System;
using System.Collections.Generic;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.IndexListGenerator.IndexPeriodGenerators
{
    internal static class DayPeriodGenerator
    {
        public static IEnumerable<DateTime> Generate(DateTime timeFrom, DateTime timeTo)
        {
            for (DateTime currentTime = timeFrom.StartOfDay(); currentTime <= timeTo; currentTime = currentTime.AddDays(1))
            {
                yield return currentTime;
            }
        }
    }
}
