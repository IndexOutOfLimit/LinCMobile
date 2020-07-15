using System;

namespace Cognizant.Hackathon.Shared.Mobile.Models
{
    public class GraphUser
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
    }
}
