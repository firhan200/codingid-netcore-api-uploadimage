using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dto;
using WebApi.Models;
using WebApi.Repositories;
using WebApi.Repositories.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductRepository _productsRepository;
        public ProductController(IProductRepository productRepository) { 
            _productsRepository = productRepository;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}";

            List<Product> products = _productsRepository.GetAll();

            foreach (var product in products)
            {
                if (product.Images != null)
                {
                    foreach (var image in product.Images)
                    {
                        if (image != null)
                        {
                            image.ImageUrl = baseUrl + "/" + image.ImageUrl;
                        }
                    }
                }
            }

            //product 1 = product image 5
            //product 2 = product image 5
            //product 3 = product image 5

            //10 product = 5 product
            //maka temen temen harus query = 11 query
            //1 query untuk get products, 10 query untuk product image
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok();
        }

        [HttpGet("Search")]
        public IActionResult SearchProduct([FromQuery] int category_id, int? product_id)
        {
            //kalo ada product_id
            return product_id != null ? Ok("Ada product ID") : Ok("Tidak Ada product ID");
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateProduct([FromBody] CreateProductDto data)
        {
            _productsRepository.Create(data.Name, data.Description, data.Price);
            return Ok();
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] CreateProductDto data)
        {
            bool isSuccess = _productsRepository.Update(id, data.Name, data.Description, data.Price);

            return Ok(new {
                Success = isSuccess,
                Id = id,
                Data = data
            });
        }
    }
}
