using System;
using System.Threading.Tasks;

namespace LinC.Platforms
{
    public interface IDownloadService
    {
        Task<string> DownloadImage(string imageName, string basePath);
    }
}
