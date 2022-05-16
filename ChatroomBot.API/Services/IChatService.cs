using ChatroomBot.API.Entities;
using ChatroomBot.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatroomBot.API.Services
{
    public interface IChatService
    {
        Task HandleMessage(MessageDto message, string user);
        IEnumerable<Message> GetMessages();
    }
}