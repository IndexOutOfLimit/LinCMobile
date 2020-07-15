using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Cognizant.Hackathon.Shared.Mobile.Core.Helpers
{
    public static class ResourceLoader
    {
        public static Stream GetEmbeddedResourceStream(Assembly assembly, string resourceFileName)
        {
            var resourceName = FormatResourceName(assembly, resourceFileName);
            return assembly.GetManifestResourceStream(resourceName);
        }
        
        public static byte[] GetEmbeddedResourceBytes(Assembly assembly, string resourceFileName)
        {
            var stream = GetEmbeddedResourceStream(assembly, resourceFileName);

            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
        
        public static string GetEmbeddedResourceString(Assembly assembly, string resourceFileName)
        {
            var stream = GetEmbeddedResourceStream(assembly, resourceFileName);

            using (var streamReader = new StreamReader(stream))
            {
                return streamReader.ReadToEnd();
            }
        }

        public static string GetEmbeddedResourceString(Type type, string resourceName)
        {
            return GetEmbeddedResourceString(type.GetTypeInfo().Assembly, resourceName);
        }

        private static string FormatResourceName(Assembly assembly, string resourceName)
        {
            resourceName = assembly.GetName()
                .Name + "." + resourceName.Replace(" ", "_")
                .Replace("\\", ".").Replace("/", ".");

            var resourceNames = assembly.GetManifestResourceNames();

            var resourcePaths = resourceNames
                .Where(x => x.EndsWith(resourceName, StringComparison.CurrentCultureIgnoreCase))
                .ToArray();

            if (!resourcePaths.Any())
                throw new Exception($"Resource ending with {resourceName} not found.");

            if (resourcePaths.Length > 1)
                throw new Exception(
                    $"Multiple resources ending with {resourceName} found: \n{string.Join(Environment.NewLine, resourcePaths)}");

            return resourcePaths.Single();

        }

        public static (string, string, string) CheckErrorResponse(string jSonResponse)
        {

            jSonResponse = jSonResponse.Replace(@"\", string.Empty);
            jSonResponse = jSonResponse.Replace("}\"", "}");
            jSonResponse = jSonResponse.Replace("\"{", "{");

            string ErrorCode = string.Empty;
            string ErrorDescription = string.Empty;
            var jsonObject = Newtonsoft.Json.Linq.JObject.Parse(jSonResponse);
            var rootData = jsonObject.SelectToken("Message.Error");
            if (rootData != null && rootData.HasValues)
            {
                ErrorCode = rootData["ErrorCode"].ToString();
                ErrorDescription = rootData["ErrorDescription"].ToString();
            }

            return (jSonResponse, ErrorCode, ErrorDescription);
        }
    }
}