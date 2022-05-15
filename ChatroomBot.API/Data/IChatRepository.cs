using ChatroomBot.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatroomBot.API.Data
{
    public interface IChatRepository
    {
        IEnumerable<Message> GetMessages();
        Task AddMessage(Message message);
    }
}