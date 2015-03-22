using System;
using System.Collections.Generic;
using ElasticSearch.Playground.Utils;

namespace ElasticSearch.Playground.Query.IndexListGenerator.IndexPeriodGenerators
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
