using ChatroomBot.API.Entities;
using ChatroomBot.API.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace ChatroomBot.API.Helpers
{
    public static class MessageMappingExtensions
    {
        public static Message ToMessageEntity(this MessageDto source, string user) => new Message
            {
                Text = source.Message,
                User = user,
                Timestamp = DateTime.UtcNow
            };
    }
}
