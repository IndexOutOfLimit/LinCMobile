using System;
namespace Cognizant.Hackathon.Shared.Mobile.Models.Models
{
    public class ChatMessage
    {
        public string Text { get; set; }
        public DateTime MessageDateTime { get; set; }
        public bool IsIncoming { get; set; }

        public string Image { get; set; }

    }
}
