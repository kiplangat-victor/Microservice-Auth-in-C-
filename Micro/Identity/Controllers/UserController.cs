using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController:ControllerBase
    {
        private static readonly string[] Users=new[]
        {
            "kiplangat","bianca","brenda","cheptoo","rian","charles","paul",
        };
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await Task.Delay(4000);
            return Ok(Users);
        }

        
    }
}