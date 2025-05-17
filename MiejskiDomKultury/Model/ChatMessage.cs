namespace MiejskiDomKultury.Models
{
    public class ChatMessage
    {
        public string Sender { get; set; }
        public string Message { get; set; }
        public bool IsUser { get; set; }

        public ChatMessage(string sender, string message, bool isUser = false)
        {
            Sender = sender;
            Message = message;
            IsUser = isUser;
        }

        public override string ToString() => $"[{Sender}]: {Message}";
    }
}
