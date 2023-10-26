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

        public List<ProductImage> GetAll()
        {
            List<ProductImage> productImages = new List<ProductImage>();

            //get connection to database
            MySqlConnection conn = new MySqlConnection(_connStr);
            try
            {
                conn.Open();
                // able to query after open
                // Perform database operations
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM product_image", conn);

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

        public void SetActive(int productId, bool isActive)
        {
            //get connection to database
            MySqlConnection conn = new MySqlConnection(_connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE product_image SET is_active=@IsActive WHERE id=@Id", conn);

                cmd.Parameters.AddWithValue("@IsActive", isActive ? 1 : 0);
                cmd.Parameters.AddWithValue("@Id", productId);

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

        public bool TestTransaction()
        {
            bool isSuccess = false;

            using(MySqlConnection conn = new MySqlConnection(_connStr))
            {
                conn.Open();

                MySqlTransaction transaction = conn.BeginTransaction();

                //start transaction
                try
                {
                    //insert new product image
                    using (var cmd = new MySqlCommand("INSERT INTO product_image(product_id, image_url) VALUES (@ProductId, @ImageUrl)", conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductId", 1);
                        cmd.Parameters.AddWithValue("@ImageUrl", "/images/testing.png");
                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();
                    }

                    //update product
                    using (var cmd = new MySqlCommand("UPDATE product SET name='[Updated]' WHERE id=@Id", conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", 1);
                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();

                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    //if fail then rollback
                    transaction.Rollback();
                }
                finally {
                    conn.Close();
                }

                return isSuccess;
            }
        }

        public void Delete(int id)
        {
            //get connection to database
            MySqlConnection conn = new MySqlConnection(_connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM product_image WHERE id=@Id", conn);

                cmd.Parameters.AddWithValue("@Id", id);

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
