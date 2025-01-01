using Frontoffice.Models;
using Microsoft.Data.SqlClient;

namespace Frontoffice.Services
{
    public class CustomerService
    {
        private readonly string _connectionString;

        public CustomerService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public Customer GetUserByEmail(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Customer WHERE CustomerEmail = @CustomerEmail", connection))
                {
                    command.Parameters.AddWithValue("@CustomerEmail", email);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Customer
                            {
                                CustomerID = (int)reader["CustomerID"],
                                CustomerEmail = reader["CustomerEmail"].ToString(),
                                CustomerFirstname = reader["CustomerFirstname"].ToString(),
                                CustomerName = reader["CustomerName"].ToString(),
                                CustomerPassword= reader["CustomerPassword"].ToString(),
                                CustomerPhone = reader["CustomerPhone"].ToString(),
                                Salt = reader["Salt"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
