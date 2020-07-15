using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Polenter.Serialization;
using Cognizant.Hackathon.Common.Helpers;

namespace Cognizant.Hackathon.Core.Common.Helpers
{
    public static class SerialiserHelper
    {
        public static byte[] ToBytes<T>(T t)
        {
            using (var stream = new MemoryStream())
            {
                try
                {
                    var serialiser = new SharpSerializer(true);
                    serialiser.Serialize(t, stream);
                    return stream.ToArray();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static T FromBytes<T>(byte[] bytes) where T : class
        {
            if (bytes == null || bytes.Length == 0)
                return default(T);

            var stream = new MemoryStream(bytes);
            stream.Position = 0;
            var serialiser = new SharpSerializer(true);
            return serialiser.Deserialize(stream) as T;
        }

        public static object FromBytes(Type type, byte[] bytes)
        {
            if (type == null || bytes == null || bytes.Length == 0)
                return null;

            var stream = new MemoryStream(bytes);
            stream.Position = 0;
            var serialiser = new SharpSerializer(true);
            return serialiser.Deserialize(stream);
        }

        public static string ToJson(this Dictionary<string, object> data)
        {
            JsonSerializer textWriter = JsonSerializer.Create(new JsonSerializerSettings());

            var sb = new StringBuilder(256);
            var sw = new StringWriter(sb, CultureInfo.InvariantCulture);

            using (var jsonWriter = new CommonTextWriter(sw))
            {
                jsonWriter.Formatting = Formatting.None;
                textWriter.Serialize(jsonWriter, data);
            }

            return sb.ToString();
        }

        public static byte[] ToByteArray(this Stream stream)
        {
            var ms = new MemoryStream();
            CopyStream(stream, ms);
            return ms.ToArray();
        }

        public static void CopyStream(Stream input, Stream output)
        {
            if(input == null)
                return;

            var buffer = new byte[16 * 1024];

            using (input)
            {
                using (output)
                {
                    int read;
                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        output.Write(buffer, 0, read);
                    }
                }
            }
        }

        public static string ToBase64String(this Stream stream)
        {
            var ms = new MemoryStream();
            CopyStream(stream, ms);
            return Convert.ToBase64String(ms.ToArray());
        }

        public static Stream FromBase64ToStream(this string base64)
        {
            var b = Convert.FromBase64String(base64);
            return new MemoryStream(b);
        }

        public static byte[] FromBase64ToByteArray(this string base64)
        {
            return Convert.FromBase64String(base64);
        }


        public static string FromByteArrayToBase64String(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
    }
}
