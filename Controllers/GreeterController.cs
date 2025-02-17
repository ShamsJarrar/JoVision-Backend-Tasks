using Microsoft.AspNetCore.Mvc;

namespace GreeterApi.Contollers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GreeterController : ControllerBase 
    {

        [HttpGet]
        public ActionResult<string> Get([FromQuery] string name = "anonymous") 
        {
            return Ok($"Hello {name}");
        }
    }
}