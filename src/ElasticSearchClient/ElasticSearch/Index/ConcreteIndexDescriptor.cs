using System;

namespace ElasticSearchClient.ElasticSearch.Index
{
    public class ConcreteIndexDescriptor : ElasticSearchIndexDescriptor
    {
        private readonly string[] concreteIndex;

        public ConcreteIndexDescriptor(string indexName, string indexTimeStampField) : base(indexTimeStampField)
        {
            if (indexName == null)
                throw new ArgumentNullException("indexName");

            concreteIndex = new[] { indexName };
        }

        public override string[] GetIndexDescriptors(DateTime fromUtcTime, DateTime toUtc)
        {
            return concreteIndex;
        }

        public override string[] GetIndexDescriptors()
        {
            return concreteIndex;
        }
    }
}