using ChatroomBot.Robot.Messages;

namespace ChatroomBot.Robot
{
    public interface IMessageBusClient
    {
        void PublishCurrentRecordValue(CurrentRecordValueMessage message); 
    }

}
