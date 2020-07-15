using System;
using System.IO;
using System.Reflection;

namespace Cognizant.Hackathon.Shared.Mobile.Data
{
    public class ReadJson
    {
        public ReadJson()
        {

        }

        public static string ReadJsonFile()
        {
            try
            {

                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(ReadJson)).Assembly;
                Stream stream = assembly.GetManifestResourceStream("Cognizant.Hackathon.Shared.Mobile.Data.JsonFile.json");
                string json;

                using (var reader = new System.IO.StreamReader(stream))
                {
                    json = reader.ReadToEnd();
                }

                return json;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }        
    }
}
