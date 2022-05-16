using ChatroomBot.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatroomBot.API.Data
{
    public class SeedData
    {
        public static void SeedMessages(AppDbContext context)
        {
            var messages = Enumerable.Range(1, 100)
                .Select(x => new Message
                {
                    User = $"user {x}",
                    Text = $"random text {x}",
                    Timestamp = DateTime.UtcNow
                });

            context.Messages.AddRange(messages);
            context.SaveChanges();
        }
    }
}
