using System.Runtime.Serialization;

namespace ElasticSearch.Client.Query.QueryGenerator.Models.Ranges
{
    [DataContract]
    public class Range
    {
        [DataMember(Name = "from")]
        public object From;
        [DataMember(Name = "to")]
        public object To;

        public Range(object @from, object to)
        {
            From = @from;
            To = to;
        }

        public Range(int @from, int to)
        {
            From = @from;
            To = to;
        }

        public Range(double @from, double to)
        {
            From = @from;
            To = to;
        }
    }
}