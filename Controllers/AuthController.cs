using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TalentMatch_AI.DTOs;
using TalentMatch_AI.Entities;

namespace TalentMatch_AI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<UserWallet> _userManager;

        public AuthController(UserManager<UserWallet> userManager)
        {
            _userManager = userManager;
        }

        // GET: api/<AuthController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AuthController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST - User Registration
        [HttpPost("registerUser")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = new UserWallet
            {
                UserName = request.UserName,
                Email = request.Email,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok(new { Message = "User registered successfully" });
        }

        // POST - User Login

        [HttpPost("loginUser")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            // Check if no email match
            if(user == null)
            {
                return Unauthorized(new { Message = "Invalid Email, No such email found." });
            }
            // Check if no password match
            if(!await _userManager.CheckPasswordAsync(user, request.Password)) {
                return Unauthorized(new { Message = "Invalid Password" });
            }

            return Ok(new { Message = "User logged in successfully" });
        }

        // PUT api/<AuthController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuthController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
