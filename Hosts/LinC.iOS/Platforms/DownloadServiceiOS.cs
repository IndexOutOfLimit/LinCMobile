using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using LinC.iOS.Platforms;
using LinC.Platforms;

[assembly: Xamarin.Forms.Dependency(typeof(DownloadServiceiOS))]
namespace LinC.iOS.Platforms
{
    public class DownloadServiceiOS : IDownloadService
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

        public async Task<bool> SaveFile(string text)
        {
            bool isSave = false;

            return isSave;
        }

    }
}
