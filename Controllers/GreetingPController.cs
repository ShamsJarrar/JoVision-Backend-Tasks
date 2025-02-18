using Microsoft.AspNetCore.Mvc;

namespace GreeterPostApi.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]
    public class GreeterPostController : ControllerBase
    {
        [HttpPost]
        public IActionResult Greeting([FromForm] string name = "anonymous") 
        {
            return Ok($"Hello {name}");
        }
    }
}