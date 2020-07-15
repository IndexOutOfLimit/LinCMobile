using System.IO;
using System.Text;
using Cognizant.Hackathon.RestClient.Helpers.Utils;

namespace Cognizant.Hackathon.RestClient.Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// Streams to string.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static string StreamToString(this Stream stream)
        {
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Combines the url base and the relative url into one, consolidating the '/' between them
        /// </summary>
        /// <param name="urlBase">Base url that will be combined</param>
        /// <param name="relativeUrl">The relative path to combine</param>
        /// <returns>The merged url</returns>
        public static string CombineUrl(
            this string urlBase,
            string relativeUrl) =>
            UrlCombine.Combine(urlBase, relativeUrl);

        /// <summary>
        /// Combines the url base and the array of relative urls into one, consolidating the '/' between them
        /// </summary>
        /// <param name="urlBase">Base url that will be combined</param>
        /// <param name="relativeUrl">The array of relative paths to combine</param>
        /// <returns>The merged url</returns>
        public static string CombineUrl(
            this string urlBase,
            params string[] relativeUrls) =>
            UrlCombine.Combine(urlBase, relativeUrls);
    }
}
