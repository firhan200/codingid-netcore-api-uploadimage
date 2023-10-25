using MySql.Data.MySqlClient;
using WebApi.Models;
using WebApi.Repositories.Interfaces;

namespace WebApi.Repositories
{
    //this class will talk/query to product table
    public class OrderRepository
    {
        private string connStr = string.Empty;

        //Dependency Injection
        public OrderRepository(IConfiguration configuration)
        {
            //di jalankan pertama kali ketika object di buat
            connStr = configuration.GetConnectionString("Default");
        }

        public void CreateOrderAndOrderDetail(int userId, List<int> productIds)
        {
            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            MySqlTransaction transaction = conn.BeginTransaction();
            try
            {
                //insert new product image
                int orderId;

                using (var cmd = new MySqlCommand("INSERT INTO orders(user_id) VALUES (@UserId)", conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Transaction = transaction;
                    cmd.ExecuteNonQuery();

                    orderId = (int)cmd.LastInsertedId;
                }

                foreach (int productId in productIds)
                {
                    using (var cmd = new MySqlCommand("INSERT INTO order_details(order_id, product_id) VALUES (@OrderId, @ProductId)", conn))
                    {
                        cmd.Parameters.AddWithValue("@OrderId", orderId);
                        cmd.Parameters.AddWithValue("@ProductId", productId);
                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();
        }
    }
}
