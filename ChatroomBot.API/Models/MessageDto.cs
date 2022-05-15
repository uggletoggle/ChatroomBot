namespace ChatroomBot.API.Models
{
    public class MessageDto
    {
        public string Message { get; set; }
        public string ExtractStockCode => ContainsStockCommand ? Message.Trim().ToLower().Substring(7) : default;
        public bool ContainsStockCommand => Message.Trim().StartsWith("/stock=");
    }
}