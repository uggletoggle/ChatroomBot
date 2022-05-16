using ChatroomBot.API.Data;
using ChatroomBot.API.Entities;
using ChatroomBot.API.Helpers;
using ChatroomBot.API.Hubs;
using ChatroomBot.API.Integration;
using ChatroomBot.API.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChatroomBot.API.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepo;
        private readonly IHubContext<ChatHub> _chatroom;
        private readonly IMessageBusClient _messageBusClient;

        public ChatService(
            IHubContext<ChatHub> chatroom,
            IMessageBusClient messageBusClient,
            IChatRepository chatRepo)
        {
            _chatRepo = chatRepo;
            _chatroom = chatroom;
            _messageBusClient = messageBusClient;
        }

        public IEnumerable<Message> GetMessages()
        {
            return _chatRepo.GetMessages();
        }

        public async Task HandleMessage(MessageDto message, string user)
        {
            var messageEntity = message.ToMessageEntity(user);
            
            if (message.ContainsStockCommand)
            {
                var stock = message.ExtractStockCode;

                if (HasValidStockCodeFormat(stock))
                {
                    try
                    {
                        _messageBusClient.PublishAskForStockMessage(new AskForStockMessage { stock = stock });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("--> Could not enqueue ask for stock message {0}", ex.Message);
                    }
                }
            }
            else
            {
                // Store message when is not stock command
                await _chatRepo.AddMessage(messageEntity);
            }

            await _chatroom.Clients.All.SendAsync("ReceiveMessage", messageEntity);
        }

        private bool HasValidStockCodeFormat(string stock) => Regex.IsMatch(stock, @"[a-z]{1,5}.[a-z]{2}");

    }
}
