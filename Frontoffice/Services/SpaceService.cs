using System.Data;
using Frontoffice.Models;
using Microsoft.Data.SqlClient;

namespace Frontoffice.Services
{
    public class SpaceService
    {
        private readonly string _connectionString;

        public SpaceService (IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public Space GetSpaceById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Space WHERE SpaceID = @SpaceID", connection))
                {
                    command.Parameters.AddWithValue("@SpaceID", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Space
                            {
                                SpaceID = (int)reader["SpaceID"],
                                SpaceCapacity = (int)reader["SpaceCapacity"],
                                SpaceName = reader["SpaceName"].ToString(),
                                SpacePrice = (double) reader["SpacePrice"],
                                Filename = reader["Filename"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }

        public DataTable GetAllSpaces()
        {
            var dataTable = new DataTable();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Space", connection);
                var adapter = new SqlDataAdapter(command);

                adapter.Fill(dataTable);
            }
            return dataTable;
        }
    }
}
