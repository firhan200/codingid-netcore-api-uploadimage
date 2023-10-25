using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dto;
using WebApi.Models;
using WebApi.Repositories;
using WebApi.Repositories.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private IProductRepository _productsRepository;
        public ProductController(IProductRepository productRepository, IWebHostEnvironment webHostEnvironment)
        {
            _productsRepository = productRepository;
            _webHostEnvironment = webHostEnvironment;
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

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto data)
        {
            /*================== upload image ==================*/
            var image = data.Image;

            var ext = Path.GetExtension(image.FileName).ToLowerInvariant(); //.jpg

            //get filename
            string fileName = Guid.NewGuid().ToString() + ext; //pasti unik
            string uploadDir = "uploads"; //foldering biar rapih
            string physicalPath = $"wwwroot/{uploadDir}";
            //saving image
            var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, physicalPath, fileName);
            using var stream = System.IO.File.Create(filePath);
            await image.CopyToAsync(stream);

            //create url path
            string fileUrlPath = $"{uploadDir}/{fileName}";
            /*================== upload image ==================*/

            _productsRepository.Create(data.Name, data.Description, data.Price, fileUrlPath);
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
