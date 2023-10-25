using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Dto;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult GetMyOrder()
        {
            string userId = User.FindFirstValue(ClaimTypes.Sid);
            string role = User.FindFirstValue(ClaimTypes.Role);
            return Ok(new { 
                Id = userId,
                Role = role
            });
        }

        [HttpPost]
        public IActionResult MakeOrder([FromBody] MakeOrderDto data)
        {
            return Ok(data);
        }
    }
}
