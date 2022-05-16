using ChatroomBot.API.Data;
using ChatroomBot.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Chatroom.Tests
{
    public class ChatRepositoryShould
    {
        private DbContextOptions<AppDbContext> _options;

        public ChatRepositoryShould()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "ChatDB").Options;

            using (var context = new AppDbContext(_options))
            {
                context.AddRange(Enumerable.Range(1, 100)
                    .Select(x => new Message
                    {
                        User = $"User {x}",
                        Text = $"Message {x}",
                        Timestamp = DateTime.UtcNow
                    }));

                context.SaveChanges();
            }
        }

        [Fact]
        public void ReturnLast50MessagesOrderByTimestampDescendant()
        {
            using (var context = new AppDbContext(_options))
            {
                ChatRepository chatRepo = new ChatRepository(context);
                List<Message> messages = chatRepo.GetMessages().ToList();
    
                
                Assert.Equal(50, messages.Count);
                Assert.True(messages[0].Timestamp > messages[1].Timestamp);
            }
        }
    }
}
