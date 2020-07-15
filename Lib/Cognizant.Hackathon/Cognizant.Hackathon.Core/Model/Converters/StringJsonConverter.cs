/*
using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using Cognizant.Hackathon.Core.Common.Helpers;

namespace Cognizant.Hackathon.Core.Model.Converters
{
    public class StringJsonConverter: JsonConverter
    {
        
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //BsonWriter
            if (writer is BsonWriter)
            {
                writer.WriteValue(value);
                return;
            }
            writer.WriteRawValue(value as string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            if (reader is BsonReader)
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    return reader.ReadAsString();
                }
                else
                {
                    return reader.Value;
                }
            }
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            return JObject.Load(reader).ToString(Formatting.None);
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(string).IsAssignableFrom(objectType);
        }

        
    }
}*/