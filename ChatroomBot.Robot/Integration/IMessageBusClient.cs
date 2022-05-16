using ChatroomBot.Common.Messages;

namespace ChatroomBot.Robot.Integration
{
    public interface IMessageBusClient
    {
        void PublishMessage<T>(T message) where T : GenericEvent; 
    }

}
