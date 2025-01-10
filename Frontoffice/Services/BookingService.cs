using System.Numerics;
using Frontoffice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Frontoffice.Services
{
    public class BookingService
    {
        private readonly string _connectionString;

        public BookingService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void CreateBooking (Booking booking) {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var command = new SqlCommand("INSERT INTO Booking (CustomerID, SpaceID, BookingDate, BookingEndDate, BookingPaidAmount, BookingPrice, IsCanceled) VALUES (@CustomerID, @SpaceID, @BookingDate, @BookingEndDate, @BookingPaidAmount, @BookingPrice, @IsCanceled)", connection);
                    command.Parameters.AddWithValue("@CustomerID", booking.CustomerID);
                    command.Parameters.AddWithValue("@SpaceID", booking.SpaceID);
                    command.Parameters.AddWithValue("@BookingDate", booking.BookingDate);
                    command.Parameters.AddWithValue("@BookingEndDate", booking.BookingEndDate);
                    command.Parameters.AddWithValue("@BookingPaidAmount", booking.BookingPaidAmount);
                    command.Parameters.AddWithValue("@BookingPrice", booking.BookingPrice);
                    command.Parameters.AddWithValue("@IsCanceled", false);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public JsonResult GetReservedDates(int spaceId)
        {
            var reservedDates = new HashSet<string>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT BookingDate, BookingEndDate FROM Booking WHERE SpaceID = @SpaceID AND IsValidated = @IsValidated";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SpaceID", spaceId);
                    command.Parameters.AddWithValue("@IsValidated", true);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime startDate = reader.GetDateTime(0);
                            DateTime endDate = reader.GetDateTime(1);

                            // Ajouter toutes les dates dans l'intervalle
                            for (var date = startDate; date <= endDate; date = date.AddDays(1))
                            {
                                reservedDates.Add(date.ToString("yyyy-MM-dd"));
                            }
                        }
                    }
                }
            }

            return new JsonResult(reservedDates);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByCustomerIdAsync(int customerId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("" +
                    "SELECT b.*, c.*, s.* " +
                        "FROM Booking b " +
                        "INNER JOIN Customer c ON b.CustomerID = c.CustomerID " +
                        "INNER JOIN Space s ON b.SpaceID = s.SpaceID " +
                        "WHERE b.CustomerID = @CustomerID", connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", customerId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        List<Booking> bookings = new List<Booking>();
                        while (await reader.ReadAsync())
                        {
                            Booking booking = new()
                            {
                                BookingID = (int)reader["BookingID"],
                                CustomerID = (int)reader["CustomerID"],
                                BookingDate = DateTime.Parse(reader["BookingDate"].ToString()),
                                BookingEndDate = DateTime.Parse(reader["BookingEndDate"].ToString()),
                                IsValidated = (bool)reader["IsValidated"],
                                SpaceID = (int)reader["SpaceID"],
                                BookingPaidAmount = 0,

                                Customer = new Customer
                                {
                                    CustomerID = (int)reader["CustomerID"],
                                    CustomerName = (reader["CustomerName"] as string),
                                    CustomerFirstname = (reader["CustomerFirstname"] as string),
                                    CustomerEmail = (reader["CustomerEmail"] as string),
                                    CustomerPhone = (reader["CustomerPhone"] as string),
                                },

                                Space = new Space
                                {
                                    SpaceID = (int)reader["SpaceID"],
                                    SpaceName = (reader["SpaceName"] as string),
                                    SpaceCapacity = (int)reader["SpaceCapacity"],
                                    SpacePrice = (double)reader["SpacePrice"],
                                    SpaceDescription = (reader["SpaceDescription"] as string),
                                    Filename = (reader["Filename"] as string)
                                }
                            };
                            Console.WriteLine(booking.ToString());
                            bookings.Add(booking);
                        }
                        return bookings;
                    }
                }
            }
        }
    }
}
