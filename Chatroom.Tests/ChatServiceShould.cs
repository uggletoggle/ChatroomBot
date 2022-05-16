using ChatroomBot.API.Data;
using ChatroomBot.API.Entities;
using ChatroomBot.API.Helpers;
using ChatroomBot.API.Hubs;
using ChatroomBot.API.Integration;
using ChatroomBot.API.Models;
using ChatroomBot.API.Services;
using ChatroomBot.Common.Messages;
using Microsoft.AspNetCore.SignalR;
using Moq;
using System;
using System.Threading;
using Xunit;

namespace Chatroom.Tests
{
    public class ChatServiceShould
    {
        Mock<IChatRepository> mockChatRepo;
        Mock<IHubContext<ChatHub>> mockHubContext;
        Mock<IMessageBusClient> mockMessageBusClient;
        public ChatServiceShould()
        {
            mockChatRepo = new Mock<IChatRepository>();
            mockHubContext = new Mock<IHubContext<ChatHub>>();
            mockMessageBusClient = new Mock<IMessageBusClient>();
        }

        [Fact]
        public void NotPersistMessageWhenIsStockCommand()
        {

            var sut = new ChatService(mockHubContext.Object,
                mockMessageBusClient.Object,
                mockChatRepo.Object);

            var messageDto = new MessageDto
            {
                Message = "/stock=stockcode"
            };

            var mockClients = new Mock<IClientProxy>();

            mockMessageBusClient.Setup(x => x.PublishAskForStockMessage(new AskForStockMessage { stock = "stockcode" }));
            mockHubContext.Setup(x => x.Clients.All).Returns(mockClients.Object);

            sut.HandleMessage(messageDto, "test user").Wait();

            mockChatRepo.Verify(x => x.AddMessage(It.IsAny<Message>()), Times.Never);
        }
    }
}
