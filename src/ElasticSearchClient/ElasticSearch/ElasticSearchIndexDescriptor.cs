using System;

namespace ElasticSearch.ElasticSearch
{
    public enum IndexStep
    {
        Hour,
        Day
    };

    public class ElasticSearchIndexDescriptor
    {
        public string IndexPrefix { get; private set; }
        public string IndexTimePattern { get; private set; }
        public string IndexTimeStampField { get; private set; }
        public IndexStep IndexStep { get; private set; }

        public bool IsAll { get; private set; }


        public ElasticSearchIndexDescriptor(string indexPrefix, string indexTimePattern, string indexTimeStampField, IndexStep indexStep)
        {
            if (indexPrefix == null) throw new ArgumentNullException("indexPrefix");
            if (indexTimePattern == null) throw new ArgumentNullException("indexTimePattern");
            if (indexTimeStampField == null) throw new ArgumentNullException("indexTimeStampField");

            IndexTimeStampField = indexTimeStampField;
            IndexPrefix = indexPrefix;
            IndexTimePattern = indexTimePattern;
            IndexStep = indexStep;
            IsAll = false;
        }
        
        /// <summary>
        /// Descriptionless index, also known as ALL
        /// </summary>
        public ElasticSearchIndexDescriptor()
        {
            IsAll = true;
        }
    }
}
