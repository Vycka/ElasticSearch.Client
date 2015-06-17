using System;
using ElasticSearch.Client.Query.QueryGenerator;
using Newtonsoft.Json;

namespace ElasticSearch.Client.Serializer.Converters
{
    public class RequestComponentConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, ((IRequestComponent)value).BuildRequestComponent());
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IRequestComponent).IsAssignableFrom(objectType);
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }
        public override bool CanRead
        {
            get
            {
                return false;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}