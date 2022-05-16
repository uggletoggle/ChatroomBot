using ChatroomBot.API.Models;
using ChatroomBot.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ChatroomBot.API.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/chatroom")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(
            IChatService chatService)
        {
            _chatService = chatService;
        }


        //[HttpPost("[action]/{connectionId}/{chatroom}")]
        //public async Task<IActionResult> JoinChat(string connectionId, string chatroom)
        //{
        //    await _chatroom.Groups.AddToGroupAsync(connectionId, chatroom);
        //    return Ok();
        //}

        //[HttpPost("[action]/{connectionId}/{chatroom}")]
        //public async Task<IActionResult> LeaveChat(string connectionId, string chatroom)
        //{
        //    await _chatroom.Groups.RemoveFromGroupAsync(connectionId, chatroom);
        //    return Ok();
        //}

        [HttpGet]
        public IActionResult GetMessages()
        {
            return Ok(_chatService.GetMessages());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendMessage([FromBody] MessageDto message)
        {
            var user = GetUserNameFromContext();
            await _chatService.HandleMessage(message, user);

            return Ok();
        }

        private string GetUserNameFromContext() => HttpContext.User.Claims
                    .Where(c => c.Type == "username")
                    .Select(c => c.Value)
                    .FirstOrDefault();

    }
}
