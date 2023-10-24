using MySql.Data.MySqlClient;
using System.Xml.Linq;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class ProductImageRepository
    {
        private readonly string _connStr;
        public ProductImageRepository(IConfiguration configuration)
        {
            //akan di jalankan pertama ketika class di buat(new ProductImageRepository())
            _connStr = configuration.GetConnectionString("Default");
        }

        public List<ProductImage> GetByProductIds(List<int> productIds)
        {
            List<ProductImage> productImages = new List<ProductImage>();

            //get connection to database
            MySqlConnection conn = new MySqlConnection(_connStr);
            try
            {
                conn.Open();
                // able to query after open
                // Perform database operations
                MySqlCommand cmd = new MySqlCommand("SELECT id, product_id, image_url FROM product_image WHERE product_id IN (@ProductIds)", conn);

                string productIdsValue = string.Join(',', productIds);

                //1, 2, 3
                //instead: [1, 2, 3]
                cmd.Parameters.AddWithValue("@ProductIds", "1,2");

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    int productIdFromTable = reader.GetInt32("product_id");
                    string imageUrl = reader.GetString("image_url");

                    productImages.Add(new ProductImage
                    {
                        Id = id,
                        ProductId = productIdFromTable,
                        ImageUrl = imageUrl
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();
            Console.WriteLine("Done.");

            return productImages;
        }

        public void Insert(int productId, string imageUrl)
        {
            //get connection to database
            MySqlConnection conn = new MySqlConnection(_connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO product_image(product_id, image_url) VALUES (@ProductId, @ImageUrl)", conn);

                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.Parameters.AddWithValue("@ImageUrl", imageUrl);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                //required
                conn.Close();
            }
        }
    }
}
