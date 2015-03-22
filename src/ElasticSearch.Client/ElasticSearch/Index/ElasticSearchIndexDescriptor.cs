using System;

namespace ElasticSearch.Client.ElasticSearch.Index
{
    public abstract class ElasticSearchIndexDescriptor
    {
        public string IndexTimeStampField { get; private set; }

        protected ElasticSearchIndexDescriptor(string indexTimeStampField)
        {
            if (indexTimeStampField == null)
                throw new ArgumentNullException("indexTimeStampField");

            IndexTimeStampField = indexTimeStampField;
        }

        public abstract string[] GetIndexDescriptors(DateTime fromUtcTime, DateTime toUtc);
        public abstract string[] GetIndexDescriptors();
    }
}