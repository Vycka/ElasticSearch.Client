using System;

namespace ElasticSearch.Client.Query.QueryGenerator.Models
{
    public enum AggregateType
    {
        Min,
        Max,
        Sum,
        Average,
        Stats,
        ExtendedStats,
        ValueCount,
        Percentiles
    }

    public static class AggregateTypeExtensions
    {
        public static string GetName(this AggregateType aggregateType)
        {
            return AggregateTypeMapping.GetName(aggregateType);
        }
    }

    public static class AggregateTypeMapping
    {
        public static string GetName(AggregateType aggregateType)
        {
            switch (aggregateType)
            {
                case AggregateType.Min:
                    return "min";
                case AggregateType.Max:
                    return "max";
                case AggregateType.Sum:
                    return "sum";
                case AggregateType.Average:
                    return "avg";
                case AggregateType.Stats:
                    return "stats";
                case AggregateType.ExtendedStats:
                    return "extended_stats";
                case AggregateType.ValueCount:
                    return "value_count";
                case AggregateType.Percentiles:
                    return "percentiles";
                default:
                    throw new ArgumentOutOfRangeException("aggregateType", aggregateType, null);
            }
        }
    }
}