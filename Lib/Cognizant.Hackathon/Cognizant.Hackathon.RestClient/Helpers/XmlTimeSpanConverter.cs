﻿using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace Cognizant.Hackathon.RestClient.Helpers
{
    /// <summary>
    /// Converter used to convert timespan to ISO8601 format
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Iso", Justification = "Iso is correct spelling")]
    public class XmlTimeSpanConverter : JsonConverter
    {
        /// <summary>
        /// Writes the specified object to JSON.
        /// </summary>
        /// <param name="writer">The JSON writer.</param>
        /// <param name="value">The value to serialize.</param>
        /// <param name="serializer">The JSON serializer.</param> 
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (serializer == null)
            {
                throw new ArgumentNullException("serializer");
            }

            //TODO: Will only be called with values of TimeSpan or TimeSpan? (Do we have to handle Nullable<TimeSpan>?)
            TimeSpan timeSpan = (TimeSpan)value;
            string xmlTimeSpanString = XmlConvert.ToString(timeSpan); //XmlConvert for TimeSpan uses ISO8601, so delegate serialization to it
            serializer.Serialize(writer, xmlTimeSpanString);
        }

        /// <summary>
        /// Reads the JSON token.
        /// </summary>
        /// <param name="reader">The JSON reader.</param>
        /// <param name="objectType">The object type.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The JSON serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            if (serializer == null)
            {
                throw new ArgumentNullException("serializer");
            }

            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            string str = serializer.Deserialize<string>(reader);
            TimeSpan timeSpan = XmlConvert.ToTimeSpan(str);
            return timeSpan;
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>true if this instance can convert the specified object type; otherwise, false.</returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TimeSpan) || objectType == typeof(TimeSpan?);
        }
    }

}
