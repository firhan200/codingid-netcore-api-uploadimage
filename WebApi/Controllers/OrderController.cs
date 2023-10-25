using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Dto;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderRepository _orderRepository;

        public OrderController(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

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

        [Authorize]
        [HttpPost]
        public IActionResult MakeOrder([FromBody] MakeOrderDto data)
        {
            string userId = User.FindFirstValue(ClaimTypes.Sid);

            _orderRepository.CreateOrderAndOrderDetail(Int32.Parse(userId), data.ProductIds);
            return Ok(data);
        }
    }
}
