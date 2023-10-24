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
        private CategoryRepository _categoryRepository;
        public ProductController(IProductRepository productRepository, CategoryRepository categoryRepository) { 
            _productsRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpGet()]
        public IActionResult GetAllProducts()
        {
            List<Product> products = _productsRepository.GetAll();

            //product 1 = product image 5
            //product 2 = product image 5
            //product 3 = product image 5

            //10 product = 5 product
            //maka temen temen harus query = 11 query
            //1 query untuk get products, 10 query untuk product image
            return Ok(products);
        }

        [HttpGet("Search")]
        public IActionResult SearchProduct()
        {
            return Ok(new {
                Results = new string[]{ "Test" }
            });
        }

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
