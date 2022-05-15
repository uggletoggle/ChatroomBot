using ChatroomBot.API.Models;

namespace ChatroomBot.API.Integration
{
    public interface IMessageBusClient
    {
        void PublishAskForStockMessage(AskForStockMessage message);
    }
}
