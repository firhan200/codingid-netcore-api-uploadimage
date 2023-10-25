using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dto;
using WebApi.Helpers;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public AuthController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDto data)
        {
            string hashedPassword = PasswordHelper.EncryptPassword(data.Password);

            User? user = _userRepository.GetByEmailAndPassword(data.Email, hashedPassword);  

            if(user == null)
            {
                return NotFound();
            }

            //create token
            string token = JWTHelper.Generate(user.Id);

            return Ok(token);
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] LoginDto data)
        {
            string hashedPassword = PasswordHelper.EncryptPassword(data.Password);

            _userRepository.Create(data.Email, hashedPassword);

            return Ok();
        }
    }
}
