using MySql.Data.MySqlClient;
using WebApi.Models;
using WebApi.Repositories.Interfaces;

namespace WebApi.Repositories
{
    //this class will talk/query to product table
    public class CategoryRepository
    {
        private string connStr = string.Empty;

        //Dependency Injection
        public CategoryRepository(IConfiguration configuration)
        {
            //di jalankan pertama kali ketika object di buat
            connStr = configuration.GetConnectionString("Default");
        }

        public List<Product> GetAll()
        {
            List<Product> products = new List<Product>();

            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                // able to query after open
                // Perform database operations
                MySqlCommand cmd = new MySqlCommand("SELECT p.id, name, description, price, pi.image_url FROM product p LEFT JOIN product_image pi ON p.id=pi.product_id", conn);

                MySqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    int id = reader.GetInt32("id");
                    string name = reader.GetString("name");
                    string description = reader.GetString("description");
                    int price = reader.GetInt32("price");
                    string imageUrl = reader.GetString("image_url");

                    Product? existingProduct = products.Where(x => x.Id == id).FirstOrDefault();
                    if (existingProduct == null)
                    {
                        products.Add(new Product
                        {
                            Id = id,
                            Name = name,
                            Description = description,
                            Price = price,
                            Images = new List<ProductImage>() { 
                                new ProductImage{ 
                                    ImageUrl = imageUrl
                                }
                            }
                        });
                    }
                    else
                    {
                        existingProduct.Images.Add(new ProductImage { 
                            ImageUrl = imageUrl
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();
            Console.WriteLine("Done.");

            return products;
        }

        public void Create(string name, string description, int price)
        {
            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO product(name, description, price) VALUES (@Name, @Description, @Price)", conn);

                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@Price", price);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();
        }

        public void Update(int id, string name, string description, int price)
        {
            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE product SET name=@Name, description=@Description, price=@Price WHERE id=@Id", conn);

                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();
        }
    }
}
