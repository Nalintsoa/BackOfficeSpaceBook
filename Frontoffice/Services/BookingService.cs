using System.Numerics;
using Frontoffice.Models;
using Microsoft.Data.SqlClient;

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
                    command.Parameters.AddWithValue("@IsCanceled", booking.IsCanceled);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
