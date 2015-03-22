using System;

namespace ElasticSearch.Client.ElasticSearch.Index
{
    public class ConcreteIndexDescriptor : ElasticSearchIndexDescriptor
    {
        private readonly string[] _concreteIndex;

        public ConcreteIndexDescriptor(string indexName, string indexTimeStampField) : base(indexTimeStampField)
        {
            if (indexName == null)
                throw new ArgumentNullException("indexName");

            _concreteIndex = new[] { indexName };
        }

        public override string[] GetIndexDescriptors(DateTime fromUtcTime, DateTime toUtc)
        {
            return _concreteIndex;
        }

        public override string[] GetIndexDescriptors()
        {
            return _concreteIndex;
        }
    }
}