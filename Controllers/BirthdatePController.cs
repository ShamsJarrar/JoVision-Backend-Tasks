using Microsoft.AspNetCore.Mvc;
using static AgeCalculator;

namespace GreeterPostApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BirthdatePostController : ControllerBase
    {
        [HttpPost]
        public IActionResult Birthdate([FromForm] string name = "anonymous", [FromForm] int? year = null, 
        [FromForm] int? month = null, [FromForm] int? day = null) {
            if(year != null && month != null && day != null) {
                int age = getAge((int)year, (int)month, (int)day);

                if(age == -1){
                    return BadRequest("Invalid date provided");
                }
                return Ok($"Hello {name}, your age is {age}");
            }
            else {
                return Ok($"Hello {name}, I can't calculate your age without knowing your birthdate!");
            }
        }
    }
}