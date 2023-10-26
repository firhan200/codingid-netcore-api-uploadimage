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
            string uploadDir = "uploads"; //foldering biar rapih
            string physicalPath = $"wwwroot/{uploadDir}";
            //saving image
            var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, physicalPath, fileName);
            using var stream = System.IO.File.Create(filePath);
            await image.CopyToAsync(stream);

            //create url path
            string fileUrlPath = $"{uploadDir}/{fileName}";

            // TODO: save product image to database
            _productImageRepository.Insert(data.ProductId, fileUrlPath);

            return Ok();
        }

        [HttpPatch("{id}/Activate")]
        public IActionResult Activate(int id)
        {
            _productImageRepository.SetActive(id, true);

            return Ok();
        }

        [HttpPatch("{id}/Deactive")]
        public IActionResult Deactive(int id)
        {
            _productImageRepository.SetActive(id, false);

            return Ok();
        }

        [HttpGet("TestTransaction")]
        public IActionResult TestTransaction()
        {
            return Ok(_productImageRepository.TestTransaction());
        }

        [HttpGet("Truncate")]
        public IActionResult Truncate()
        {
            List<ProductImage> productImages = _productImageRepository.GetAll();
            string rootPath = _webHostEnvironment.WebRootPath;

            foreach (ProductImage productImage in productImages)
            {
                if (!string.IsNullOrEmpty(productImage.ImageUrl))
                {
                    //TODO: 1. get physical path
                    string imagePath = Path.Combine(rootPath, productImage.ImageUrl);
                    //TODO: 2. remove image from server
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    //TODO: 3. delete from database
                    _productImageRepository.Delete(productImage.Id);
                }
            }

            return Ok();
        }
    }
}
