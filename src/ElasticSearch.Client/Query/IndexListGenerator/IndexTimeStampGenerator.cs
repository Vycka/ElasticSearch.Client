using System;
using System.Collections.Generic;
using System.ComponentModel;
using ElasticSearch.Playground.ElasticSearch.Index;
using ElasticSearch.Playground.Query.IndexListGenerator.IndexPeriodGenerators;

namespace ElasticSearch.Playground.Query.IndexListGenerator
{
    public static class IndexTimeStampGenerator
    {
        public static IEnumerable<DateTime> Generate(DateTime timeFrom, DateTime timeTo, IndexStep indexStep)
        {
            switch (indexStep)
            {
                case IndexStep.Day:
                    return DayPeriodGenerator.Generate(timeFrom, timeTo);
                case IndexStep.Hour:
                    return HourPeriodGenerator.Generate(timeFrom, timeTo);
                default:
                    throw new InvalidEnumArgumentException("Unsupported IndexStep provided");
            }
        }
    }
}
