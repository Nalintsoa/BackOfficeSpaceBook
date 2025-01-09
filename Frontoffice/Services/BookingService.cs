using System.Numerics;
using Frontoffice.Models;
using Microsoft.AspNetCore.Mvc;
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

        public JsonResult GetReservedDates(int spaceId)
        {
            var reservedDates = new HashSet<string>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT BookingDate, BookingEndDate FROM Booking WHERE SpaceID = @SpaceID";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SpaceID", spaceId);

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
    }
}
