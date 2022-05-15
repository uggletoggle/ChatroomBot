using System;
using System.Threading.Tasks;
using ChatroomBot.API.Data;
using ChatroomBot.API.Entities;
using Microsoft.AspNetCore.SignalR;

namespace ChatroomBot.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _context;

        public ChatHub(AppDbContext context)
        {
            _context = context;
        }

        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("ReceiveConnID", Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public async Task SendMessage(string user, string message)
        {
            var messageEntity = new Message
            {
                User = user,
                Text = message,
                Timestamp = DateTime.UtcNow
            };
            await _context.Messages.AddAsync(messageEntity);

            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
