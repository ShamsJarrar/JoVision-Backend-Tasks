using System;
using Microsoft.AspNetCore.Mvc;

namespace GreeterApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BirthDateController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get([FromQuery] string name = "anonymous", int? year = null, 
        int? month = null, int? day = null) 
        {
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

        private int getAge(int year, int month, int day) {
            try {
                DateTime birthDate = new DateTime(year, month, day);
                DateTime today = DateTime.Today;

                int age = today.Year - birthDate.Year;
                if((today.Month < birthDate.Month) ||
                (today.Month == birthDate.Month &&
                today.Day < birthDate.Day)) {
                    age--;
                }
                
                return age;
            }
            catch {
                return -1;
            }
        }
    }
}