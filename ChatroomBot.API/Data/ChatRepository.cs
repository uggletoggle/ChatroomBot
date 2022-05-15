using ChatroomBot.API.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatroomBot.API.Data
{
    public class ChatRepository : IChatRepository
    {
        private readonly AppDbContext _context;

        public ChatRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddMessage(Message message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Message> GetMessages()
        {
            var messages = _context.Messages
                .OrderByDescending(m => m.Timestamp)
                .Take(50);

            return messages;
        }

    }
}
