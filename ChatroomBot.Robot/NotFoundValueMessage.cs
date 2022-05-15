namespace ChatroomBot.Robot
{
    public class NotFoundValueMessage : GenericEvent
    {
        public NotFoundValueMessage()
        {
            Event = "NotFoundValue";
        }
        public string symbol { get; set; }
    }
}