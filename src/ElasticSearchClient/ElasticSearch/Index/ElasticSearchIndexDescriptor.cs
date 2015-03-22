using System;

namespace ElasticSearchClient.ElasticSearch.Index
{
    public abstract class ElasticSearchIndexDescriptor
    {
        private readonly string _indexTimeStampField;
        public string IndexTimeStampField { get; private set; }

        protected ElasticSearchIndexDescriptor(string indexTimeStampField)
        {
            if (indexTimeStampField == null)
                throw new ArgumentNullException("indexTimeStampField");

            _indexTimeStampField = indexTimeStampField;
        }

        public abstract string[] GetIndexDescriptors(DateTime fromUtcTime, DateTime toUtc);
        public abstract string[] GetIndexDescriptors();
    }
}