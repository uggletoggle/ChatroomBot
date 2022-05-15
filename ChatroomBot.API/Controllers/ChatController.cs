using ChatroomBot.API.Data;
using ChatroomBot.API.Entities;
using ChatroomBot.API.Hubs;
using ChatroomBot.API.Integration;
using ChatroomBot.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChatroomBot.API.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/chatroom")]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _chatroom;
        private readonly IMessageBusClient _messageBusClient;

        public ChatController(
            IMessageBusClient messageBusClient,
            IHubContext<ChatHub> chatroom)
        {
            _chatroom = chatroom;
            _messageBusClient = messageBusClient;
        }

        [HttpPost("[action]/{connectionId}/{chatroom}")]
        public async Task<IActionResult> JoinChat(string connectionId, string chatroom)
        {
            await _chatroom.Groups.AddToGroupAsync(connectionId, chatroom);
            return Ok();
        }

        [HttpPost("[action]/{connectionId}/{chatroom}")]
        public async Task<IActionResult> LeaveChat(string connectionId, string chatroom)
        {
            await _chatroom.Groups.RemoveFromGroupAsync(connectionId, chatroom);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendMessage([FromBody] MessageDto message,
            [FromServices] AppDbContext context)
        {
            var isAskingForStock = message.Message
                .Trim()
                .StartsWith("/stock=");

            var messageEntity = new Message
            {
                Text = message.Message,
                User = HttpContext.User.Claims
                    .Where( c => c.Type == "username")
                    .Select( c => c.Value)
                    .FirstOrDefault(),
                Timestamp = DateTime.UtcNow
            };

            if (isAskingForStock)
            {
                var stock = message.Message
                .Trim().ToLower().Substring(7);

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
                context.Messages.Add(messageEntity);
                await context.SaveChangesAsync();
            }
            
            await _chatroom.Clients.All.SendAsync("ReceiveMessage",  messageEntity);
            return Ok();
        }

        private bool HasValidStockCodeFormat(string stock)
        {
            return Regex.IsMatch(stock, @"[a-z]{1,5}.[a-z]{2}");
        }
    }
}
