using Microsoft.VisualBasic;

namespace WebApi.Models
{
    //contains all product columns/properties
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Price { get; set; }

        public string Image { get; set; } = string.Empty;

        public List<ProductImage> Images { get; set; }
    }
}
