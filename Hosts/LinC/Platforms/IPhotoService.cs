using System;
using System.Threading.Tasks;

namespace LinC.Platforms
{
    public interface IPhotoService
    {
        Task<bool> CheckGalleryPermissionStatus();
        Task<bool> CheckCameraPermissionStatus();
    }
}
