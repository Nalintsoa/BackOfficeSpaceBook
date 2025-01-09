using System.Data;
using Frontoffice.Models;
using Frontoffice.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Frontoffice.Controllers
{
    public class SpaceController : Controller
    {
        private readonly SpaceService _spaceService;
        private readonly SharedFileService _sharedFileService;
        private readonly BookingService _bookingService;
        private readonly IEmailSender _emailSender;

        public SpaceController(SpaceService spaceService, SharedFileService sharedFileService, BookingService bookingService, IEmailSender emailSender)
        {
            _spaceService = spaceService;
            _sharedFileService = sharedFileService;
            _bookingService = bookingService;
            _emailSender = emailSender;
        }

        // GET: SpaceController
        public ActionResult Index()
        {
            var dataTable = _spaceService.GetAllSpaces();
            foreach (System.Data.DataRow row in dataTable.Rows)
            {
                row["Filename"] = _sharedFileService.GetSharedFilePath(row["Filename"].ToString());
            }
            return View(dataTable);
        }

        // GET: SpaceController/Details/5
        public ActionResult Details(int id)
        {
            var space = _spaceService.GetSpaceById(id);
            if (space == null)
            {
                return NotFound();
            }

            space.Filename = _sharedFileService.GetSharedFilePath(space.Filename);
            return View(space);
        }

        public async Task<ActionResult> CreateBooking(string date, string space, string end)
        {
            DateTime endDate = ParseBookingDateToString(end);
            DateTime beginDate = ParseBookingDateToString(date);
            System.TimeSpan dateDiff = endDate - beginDate;

            var booking = new Booking()
            {
                CustomerID = int.Parse(HttpContext.Session.GetString("customerID")),
                BookingDate = beginDate,
                BookingEndDate = endDate,
                BookingPaidAmount = 0,
                BookingPrice = 0,
                SpaceID = int.Parse(space),
                IsCanceled = false,
            };

            
            string customerEmail = "nalytovo@gmail.com";
            string subject = "Votre réservation a été créée";
            string body = $"Bonjour, votre réservation de {dateDiff.Days} jours pour l'espace {space} a été créée. " +
                          $"Date de début: {booking.BookingDate}, Date de fin: {booking.BookingEndDate}. " +
                          $"Nous vous contacterons pour plus de détails.";
            await _emailSender.SendEmailAsync(customerEmail, subject, body);

            _bookingService.CreateBooking(booking);

            return RedirectToAction("Index", "Customer");
        }

        private static DateTime ParseBookingDateToString (string date)
        {
            DateTime bookingDate = DateTime.Now;
            var isValidDate = DateTime.TryParse(date, out bookingDate);
            DateTime bookingDt = isValidDate ? bookingDate : DateTime.Now;

            return bookingDt;
        }

        public JsonResult GetBookingsDate (int SpaceID)
        {
            return _bookingService.GetReservedDates(SpaceID);
        }
    }
}
