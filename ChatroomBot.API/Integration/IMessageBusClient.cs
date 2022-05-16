using ChatroomBot.API.Models;
using ChatroomBot.Common.Messages;

namespace ChatroomBot.API.Integration
{
    public interface IMessageBusClient
    {
        void PublishAskForStockMessage(AskForStockMessage message);
    }
}
