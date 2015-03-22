using System;

namespace ElasticSearchClient.ElasticSearch
{
    public enum IndexStep
    {
        Hour,
        Day
    };

    public class IndexDescriptor
    {
        public string IndexPrefix { get; private set; }
        public string IndexTimePattern { get; private set; }
        public string IndexTimeStampField { get; private set; }
        public IndexStep IndexStep { get; private set; }

        public IndexDescriptor(string indexPrefix, string indexTimePattern, string indexTimeStampField, IndexStep indexStep)
        {
            if (indexPrefix == null) throw new ArgumentNullException("indexPrefix");
            if (indexTimePattern == null) throw new ArgumentNullException("indexTimePattern");
            if (indexTimeStampField == null) throw new ArgumentNullException("indexTimeStampField");

            IndexTimeStampField = indexTimeStampField;
            IndexPrefix = indexPrefix;
            IndexTimePattern = indexTimePattern;
            IndexStep = indexStep;
        }
    }
}
