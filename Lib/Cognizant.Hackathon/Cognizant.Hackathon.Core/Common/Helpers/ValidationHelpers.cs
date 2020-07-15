using System;
using System.Text.RegularExpressions;

namespace Cognizant.Hackathon.Core.Common.Helpers
{
    public class ValidationHelpers
    {
        public static bool IsValidEmail(string username)
        {
            var isEmail = false;

            if (!String.IsNullOrEmpty(username))
            {
                isEmail = Regex.IsMatch(username, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            }

            return isEmail;
        }
    }
}