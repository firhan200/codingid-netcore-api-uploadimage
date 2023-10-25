namespace WebApi.Dto
{
    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Price { get; set; }
        public IFormFile Image { get; set; }
    }
}
