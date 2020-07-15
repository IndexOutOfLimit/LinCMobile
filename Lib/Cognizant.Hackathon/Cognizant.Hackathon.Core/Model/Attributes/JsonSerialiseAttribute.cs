using System;
using System.Collections.Generic;
using System.Linq;
using Cognizant.Hackathon.Core.Common.Enum;

namespace Cognizant.Hackathon.Core.Model.Attributes
{
    public class JsonSerialiseAttribute : Attribute
    {
        public JsonSerialiseAttribute(params UserRole[] allowedRoles)
        {
            if(allowedRoles.Contains(UserRole.None))
            {
                AllowedRoles = new List<UserRole>();
                return;
            }
            
            AllowedRoles = allowedRoles.ToList();

            if (!AllowedRoles.Contains(UserRole.Realogy))
                AllowedRoles.Add(UserRole.Realogy);
        }

        public List<UserRole> AllowedRoles { get; private set; }
    }
}