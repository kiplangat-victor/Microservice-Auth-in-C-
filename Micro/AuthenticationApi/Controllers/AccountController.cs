// using System.Collections.Concurrent;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.IdentityModel.Tokens;

// namespace AuthenticationApi.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]

//     public class AccountController(IConfiguration config):ControllerBase
//     {
        
//         private static ConcurrentDictionary<string, string> UserData { get; set; }=new ConcurrentDictionary<string, string>();
        

//         [HttpPost("login/{email}/{password}")]
        
//         public async Task<IActionResult> Login(string email,string password)
//         {
//             await Task.Delay(500);
//             var getEmail=UserData!.Keys.Where(e=>e.Equals(email)).FirstOrDefault();
//             if (!string.IsNullOrEmpty(getEmail))
//             {
//                 UserData.TryGetValue(getEmail, out string? dbPassword);
//                 if (!Equals(dbPassword,password))
//                 return BadRequest("Invalid Credentials");
//                 string jwtToken=GenerateToken(getEmail);
//                 return Ok(jwtToken);

//             }
//             return NotFound("Email not found");

//         }

//         private string GenerateToken(string getEmail)
//         {
//             var key=Encoding.UTF8.GetBytes(config["Authentication:Key"]!);
//             var securityKey=new SymmetricSecurityKey(key);
//             var credentials=new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
//             var claims=new[]{new Claim(ClaimTypes.Email,getEmail!) };
//             var token=new JwtSecurityToken(
//                 issuer:config["Authentication:Issuer"],
//                 audience:config["Authenticatio:Audience"],
//                 claims:claims,
//                 expires:null,
//                 signingCredentials:credentials
//             );
//             return new JwtSecurityTokenHandler().WriteToken(token);
//         }
//         [HttpPost("register/{email}/{password}")]
//         public async Task<IActionResult> Register(string email,string password)
//         {
//             await Task.Delay(500);
//             var getEmail=UserData!.Keys.Where(e=>e.Equals(email)).FirstOrDefault();
//             if(!string.IsNullOrEmpty(getEmail))
//             return BadRequest("User already exist");

//             UserData[email]=password;
//             return Ok("User Created Successfully");
//         }
//     }
// }
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace AuthenticationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _config;

        // Constructor for injecting IConfiguration
        public AccountController(IConfiguration config)
        {
            _config = config;
        }

        private static ConcurrentDictionary<string, string> UserData { get; set; } = new ConcurrentDictionary<string, string>();

        [HttpPost("login/{email}/{password}")]
        public async Task<IActionResult> Login(string email, string password)
        {
            await Task.Delay(500);  // Simulating a delay for async operations
            var getEmail = UserData.Keys.FirstOrDefault(e => e.Equals(email));
            if (!string.IsNullOrEmpty(getEmail))
            {
                if (UserData.TryGetValue(getEmail, out string? dbPassword))
                {
                    if (dbPassword == password)
                    {
                        string jwtToken = GenerateToken(getEmail);
                        return Ok(jwtToken);
                    }
                }
                return BadRequest("Invalid Credentials");
            }
            return NotFound("Email not found");
        }

        private string GenerateToken(string email)
        {
            var key = Encoding.UTF8.GetBytes(_config["Authentication:Key"]!);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] { new Claim(ClaimTypes.Email, email) };
            var token = new JwtSecurityToken(
                issuer: _config["Authentication:Issuer"],
                audience: _config["Authentication:Audience"],
                claims: claims,
                expires: null,
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("register/{email}/{password}")]
        public async Task<IActionResult> Register(string email, string password)
        {
            await Task.Delay(500);  // Simulating a delay for async operations
            var getEmail = UserData.Keys.FirstOrDefault(e => e.Equals(email));
            if (!string.IsNullOrEmpty(getEmail))
                return BadRequest("User already exists");

            UserData[email] = password;
            return Ok("User Created Successfully");
        }
    }
}
