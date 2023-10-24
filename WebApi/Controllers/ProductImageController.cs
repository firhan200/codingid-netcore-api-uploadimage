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
    public class ProductImageController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProductRepository _productRepository;
        private readonly ProductImageRepository _productImageRepository;

        public ProductImageController(IProductRepository productRepository, IWebHostEnvironment webHostEnvironment, ProductImageRepository productImageRepository)
        {
            _productRepository = productRepository;
            _webHostEnvironment = webHostEnvironment;
            _productImageRepository = productImageRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductImage([FromForm] CreateProductImageDto data)
        {
            // TODO: check if product exist
            Product? product = _productRepository.GetById(data.ProductId);
            if(product == null)
            {
                return NotFound();
            }

            if(data.Image == null)
            {
                return BadRequest();
            }

            IFormFile image = data.Image!;

            // TODO: save image to server
            var ext = Path.GetExtension(image.FileName).ToLowerInvariant(); //.jpg

            //get filename
            string fileName = Guid.NewGuid().ToString() + ext; //pasti unik
            string uploadDir = "images"; //foldering biar rapih
            string physicalPath = $"wwwroot/{uploadDir}";
            //saving image
            var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, physicalPath, fileName);
            using var stream = System.IO.File.Create(filePath);
            await image.CopyToAsync(stream);

            //create url path
            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            string fileUrlPath = $"{baseUrl}/{uploadDir}/{fileName}";

            // TODO: save product image to database
            _productImageRepository.Insert(data.ProductId, fileUrlPath);

            return Ok();
        }
    }
}
