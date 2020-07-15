using System.Collections.Generic;
using Cognizant.Hackathon.Core.Model.Internal;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;

namespace Cognizant.Hackathon.Shared.Mobile.Models
{
    public class Group : CoreObject
    {
        public string Name { get; set; }
        public virtual ICollection<LinCUser> Users { get; set; }
    }
}