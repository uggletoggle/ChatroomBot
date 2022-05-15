using ChatroomBot.API.Integration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatroomBot.API.Controllers
{
    [ApiController]
    [Route("api/foo")]
    public class FooController : ControllerBase
    {
        private readonly IMessageBusClient _messageBusClient;

        public FooController(IMessageBusClient messageBusClient)
        {
            _messageBusClient = messageBusClient;
        }
        //[Authorize]
        //[HttpGet]
        //public IActionResult Index()
        //{
        //    return Ok();
        //}

        [HttpGet]
        public IActionResult SendMessage()
        {
            _messageBusClient.PublishAskForStockMessage(new Models.AskForStockMessage
            {
                stock = "APPL.US"
            });

            return Ok();
        }
    }
}
