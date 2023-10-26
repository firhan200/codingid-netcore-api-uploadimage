using MySql.Data.MySqlClient;
using WebApi.Models;
using WebApi.Repositories.Interfaces;

namespace WebApi.Repositories
{
    //this class will talk/query to product table
    public class UserRepository
    {
        private string connStr = string.Empty;

        //Dependency Injection
        public UserRepository(IConfiguration configuration)
        {
            //di jalankan pertama kali ketika object di buat
            connStr = configuration.GetConnectionString("Default");
        }

        public User? GetByEmailAndPassword(string email, string password)
        {
            User? user = null;

            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT id, email, role FROM user WHERE email=@Email and password=@Password", conn);

                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                var reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    user = new User { 
                        Id = reader.GetInt32("id"),
                        Email = reader.GetString("email"),
                        Role = reader.GetString("role"),
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();

            return user;
        }

        public User? GetByEmail(string email)
        {
            User? user = null;

            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT id, email, role FROM user WHERE email=@Email", conn);

                cmd.Parameters.AddWithValue("@Email", email);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    user = new User
                    {
                        Id = reader.GetInt32("id"),
                        Email = reader.GetString("email"),
                        Role = reader.GetString("role"),
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();

            return user;
        }

        public User? GetByEmailAndResetToken(string email, string resetToken)
        {
            User? user = null;

            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT id, email, role FROM user WHERE email=@Email AND reset_password_token=@ResetToken", conn);

                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@ResetToken", resetToken);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    user = new User
                    {
                        Id = reader.GetInt32("id"),
                        Email = reader.GetString("email"),
                        Role = reader.GetString("role"),
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();

            return user;
        }

        public void Create(string email, string password)
        {
            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO user(email, password) VALUES (@Email, @Password)", conn);

                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();
        }

        public bool InsertResetPasswordToken(int userId, string token)
        {
            bool isSuccess = false;

            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE user SET reset_password_token=@Token WHERE id=@Id", conn);

                cmd.Parameters.AddWithValue("@Id", userId);
                cmd.Parameters.AddWithValue("@Token", token);

                cmd.ExecuteNonQuery();

                isSuccess = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();

            return isSuccess;
        }

        public bool UpdatePassword(int userId, string newPassword)
        {
            bool isSuccess = false;

            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE user SET password=@NewPassword WHERE id=@Id", conn);

                cmd.Parameters.AddWithValue("@Id", userId);
                cmd.Parameters.AddWithValue("@NewPassword", newPassword);

                cmd.ExecuteNonQuery();

                isSuccess = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();

            return isSuccess;
        }
    }
}
