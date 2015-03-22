using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ElasticSearch.Playground.Query.IndexListGenerator;

namespace ElasticSearch.Playground.ElasticSearch.Index
{
    public class TimeStampedIndexDescriptor : ElasticSearchIndexDescriptor
    {
        public string IndexPrefix { get; private set; }
        public string IndexTimePattern { get; private set; }

        public IndexStep IndexStep { get; private set; }

        public TimeStampedIndexDescriptor(string indexPrefix, string indexTimePattern, string indexTimeStampField, IndexStep indexStep)
            : base(indexTimeStampField)
        {
            if (indexPrefix == null) throw new ArgumentNullException("indexPrefix");
            if (indexTimePattern == null) throw new ArgumentNullException("indexTimePattern");
            if (indexTimeStampField == null) throw new ArgumentNullException("indexTimeStampField");

            IndexPrefix = indexPrefix;
            IndexTimePattern = indexTimePattern;
            IndexStep = indexStep;
        }

        public override string[] GetIndexDescriptors(DateTime fromUtcTime, DateTime toUtc)
        {
            IEnumerable<DateTime> timeStamps = IndexTimeStampGenerator.Generate(fromUtcTime, toUtc, IndexStep);
            string[] result =  timeStamps.Select(ts => 
                string.Concat(
                    IndexPrefix,
                    ts.ToString(IndexTimePattern, CultureInfo.InvariantCulture)
                )
            )
            .ToArray();

            return result;
        }

        public override string[] GetIndexDescriptors()
        {
            return new [] { IndexPrefix + "*" };
        }
    }

    public enum IndexStep
    {
        Hour,
        Day
    };
}