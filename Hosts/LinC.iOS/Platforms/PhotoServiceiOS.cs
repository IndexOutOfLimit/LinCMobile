using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using LinC.iOS.Platforms;
using LinC.Platforms;
[assembly: Xamarin.Forms.Dependency(typeof(PhotoServiceiOS))]
namespace LinC.iOS.Platforms
{
    public class PhotoServiceiOS : IPhotoService
    {
        public PhotoServiceiOS()
        {
        }

        public async Task<bool> CheckCameraPermissionStatus()
        {
            var hasPermission = await RequestCameraPermissionAsync();
            return hasPermission;

        }

        public async Task<bool> CheckGalleryPermissionStatus()
        {
            var hasPermission = await RequestGalleryPermissionAsync();
            return hasPermission;
            
        }

        public async Task<bool> RequestGalleryPermissionAsync()
        {
            bool permissionGranted = false;
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);

                 var authotization = new Dictionary<Permission, PermissionStatus>();

            if (status == PermissionStatus.Unknown)
            {
                authotization = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Photos);


                foreach (var item in authotization)
                {
                    if (item.Key.ToString().ToLower().Contains("photos"))
                    {
                        permissionGranted = item.Value.ToString().ToLower().Contains("granted") ? true : false;
                       
                    }

                }
            }
            else if(status == PermissionStatus.Granted)
            {
                permissionGranted = true;
            }
            else
            {
                permissionGranted = false;
            }
            return permissionGranted;
        }

        public async Task<bool> RequestCameraPermissionAsync()
        {
            bool permissionGranted = false;
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);

            var authotization = new Dictionary<Permission, PermissionStatus>();

            if (status == PermissionStatus.Unknown)
            {
                authotization = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);


                foreach (var item in authotization)
                {
                    if (item.Key.ToString().ToLower().Contains("camera"))
                    {
                        permissionGranted = item.Value.ToString().ToLower().Contains("granted") ? true : false;

                    }

                }
            }
            else if (status == PermissionStatus.Granted)
            {
                permissionGranted = true;
            }
            else
            {
                permissionGranted = false;
            }
            return permissionGranted;
        }
    }
}
