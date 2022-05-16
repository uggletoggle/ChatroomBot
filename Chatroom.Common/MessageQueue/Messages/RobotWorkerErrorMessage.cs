namespace ChatroomBot.Common.Messages
{
    public class RobotWorkerErrorMessage : GenericEvent
    {
        public RobotWorkerErrorMessage()
        {
            Event = "RobotWorkerError";
        }
        public string ExceptionType { get; set; }
        public string Description { get; set; }
    }
}
