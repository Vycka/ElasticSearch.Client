using System.Runtime.Serialization;

namespace ElasticSearch.Client.ElasticSearch.Results
{
    [DataContract]
    public class ResultItem<TResultItem>
    {
        [DataMember(Name = "_index")]
        public string Index { get; set; }

        [DataMember(Name = "_type")]
        public string Type { get; set; }

        [DataMember(Name = "_id")]
        public string Id { get; set; }

        [DataMember(Name = "_source")]
        public TResultItem Source { get; set; }

        public override string ToString()
        {
            return string.Format("{0}/{1}/{2}", Index, Type, Id);
        }
    }
}
