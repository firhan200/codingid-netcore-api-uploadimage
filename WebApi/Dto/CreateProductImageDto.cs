namespace WebApi.Dto
{
    public class CreateProductImageDto
    {
        public int ProductId { get; set; }
        public IFormFile? Image { get; set; } //ini untuk nerima file
    }
}
