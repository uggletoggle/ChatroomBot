using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatroomBot.API.Controllers
{
    [ApiController]
    [Route("api/foo")]
    public class FooController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
