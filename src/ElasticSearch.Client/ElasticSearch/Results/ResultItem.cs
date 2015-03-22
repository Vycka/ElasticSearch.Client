using System.Runtime.Serialization;

namespace ElasticSearch.Client.ElasticSearch.Results
{
    [DataContract]
    public class ResultItem
    {
        [DataMember(Name = "_index")]
        public string Index { get; set; }

        [DataMember(Name = "_type")]
        public string Type { get; set; }

        [DataMember(Name = "_id")]
        public string Id { get; set; }

        [DataMember(Name = "_source")]
        public dynamic Source { get; set; }
     }
}
