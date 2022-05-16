namespace ChatroomBot.Common.Messages
{
    public class NotFoundValueMessage : GenericEvent
    {
        public NotFoundValueMessage()
        {
            Event = "NotFoundValue";
        }
        public string Symbol { get; set; }
    }
}