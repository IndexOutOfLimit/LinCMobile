using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using LinC.Droid.Platforms;
using LinC.Platforms;

[assembly: Xamarin.Forms.Dependency(typeof(DownloadServiceDroid))]
namespace LinC.Droid.Platforms
{
    public class DownloadServiceDroid : IDownloadService
    {
        public async Task<string> DownloadImage(string imageName, string imageUrl)
        {
            string localPath = string.Empty;

            try
            {
                var webClient = new WebClient();
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string localFilename = imageName;
                localPath = Path.Combine(documentsPath, localFilename);

                webClient.DownloadDataCompleted += (s, e) => {
                    try
                    {
                        var bytes = e.Result; // get the downloaded data
                        File.WriteAllBytes(localPath, bytes); // writes to local storage
                    }
                    catch (Exception)
                    {

                    }
                };

                var url = new Uri(imageUrl);
                await webClient.DownloadDataTaskAsync(url);
            }
            catch (Exception)
            {
                localPath = string.Empty;
            }
            

            return localPath;
        }
    }
}
