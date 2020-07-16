using System;
using System.Linq;
using System.Reflection;
using Cognizant.Hackathon.Core.Common.Helpers;

namespace Cognizant.Hackathon.Core.Common.Enum
{  
    [Flags]
    public enum UserRole
    {
       
        None = 0,      
        Realogy = 1 << 0,        
        Consumer = 1 << 2,
        Unregistered = 1 << 3,
        [ParentUserRole(Realogy)]
        System = 1 << 4,

        [ParentUserRole(Realogy)]
        RealogyMasterAdmin = 1 << 5,
        [ParentUserRole(Realogy)]
        RealogyAdmin = 1 << 6,
        [ParentUserRole(Realogy)]
        RealogySupport = 1 << 7,
        [ParentUserRole(Consumer)]
        Member = 1 << 12,
        [ParentUserRole(Consumer)]
        TemporaryMember = 1 << 13,
        [ParentUserRole(Consumer)]
        Anonymous = 1 << 14,
        [ParentUserRole(Consumer)]
        Reset = 1 << 15,

    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ParentUserRoleAttribute : Attribute
    {
        public ParentUserRoleAttribute(UserRole parentUserRole)
        {
            ParentUserRole = parentUserRole;
        } 
      
        public UserRole ParentUserRole { get; set; }
        
    }

    public static class UserRoleExtensions
    {         
        public static UserRole? GetParentUserRole(this UserRole UserRole)
        {
            UserRole? result = null;
            Type type = UserRole.GetType();
            FieldInfo field = type.GetRuntimeField(UserRole.ToString());
            var ptcas = field.GetCustomAttributes(typeof(ParentUserRoleAttribute), false) as ParentUserRoleAttribute[];
            if (ptcas.Length > 0)
            {
                result = ptcas.First().ParentUserRole;
            }

            return result;
        }        
    }
}
