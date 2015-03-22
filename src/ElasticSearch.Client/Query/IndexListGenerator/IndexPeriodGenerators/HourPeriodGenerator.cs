using System;
using System.Collections.Generic;
using ElasticSearch.Playground.Utils;

namespace ElasticSearch.Playground.Query.IndexListGenerator.IndexPeriodGenerators
{
    internal static class HourPeriodGenerator
    {
        public static IEnumerable<DateTime> Generate(DateTime timeFrom, DateTime timeTo)
        {
            for (DateTime currentTime = timeFrom.StartOfHour(); currentTime <= timeTo; currentTime = currentTime.AddHours(1))
            {
                yield return currentTime;
            }
        }
    }
}
