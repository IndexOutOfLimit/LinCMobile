using System.Collections.Generic;
using Cognizant.Hackathon.Shared.Mobile.Core.Interfaces;
using Cognizant.Hackathon.Shared.Mobile.Models.Enums;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;
using Cognizant.Hackathon.Shared.Mobile.Shared.Helpers;

namespace LinC.Helpers
{
    public class PopUpHelper
    {
        public static List<object> GetMoreMenuOptionList(ILinCApiServices services)
        {
            var moreMenuList = new List<object>();

            //moreMenuList.Add(new Item() { Text = "Settings", ImageName = FontAwesomeIcons.Cog });
            //moreMenuList.Add(new Item() { Text = "Logout", ImageName = FontAwesomeIcons.Building });


            return moreMenuList;
        }           

        public static List<object> GetImageCaptureOptions(ILinCApiServices services)
        {
            var imageCaptureOptions = new List<object>();
            imageCaptureOptions.Add(new Item() { Text = "Gallery", ImageName = FontAwesomeIcons.User });
            imageCaptureOptions.Add(new Item() { Text = "Camera", ImageName = FontAwesomeIcons.UserPlus });

            return imageCaptureOptions;
        }
    }
}
