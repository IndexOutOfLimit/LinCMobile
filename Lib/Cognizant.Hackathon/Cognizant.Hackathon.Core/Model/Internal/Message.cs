namespace Cognizant.Hackathon.Core.Model.Internal
{
    public class Message<T>
    {
        public Message()
        {
            
        }
        public Message(T body) : this()
        {
            Body = body;
        }

        public T Body { get; set; }

        public string MessageId { get; set; }

    }
}