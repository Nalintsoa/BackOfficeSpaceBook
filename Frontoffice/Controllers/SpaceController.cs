﻿using System.Data;
using Frontoffice.Helpers;
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
        private readonly CustomerService _customerService;

        public SpaceController(SpaceService spaceService, SharedFileService sharedFileService, BookingService bookingService, IEmailSender emailSender, CustomerService customerService)
        {
            _spaceService = spaceService;
            _sharedFileService = sharedFileService;
            _bookingService = bookingService;
            _emailSender = emailSender;
            _customerService = customerService;
        }

        // GET: SpaceController
        public ActionResult Index(string search = "", int page = 1)
        {
            int pageSize = 6; 
            var dataTable = _spaceService.GetAllSpaces();

      
            if (!string.IsNullOrEmpty(search))
            {
                dataTable = dataTable.AsEnumerable()
                                     .Where(row => row["SpaceName"].ToString().Contains(search, StringComparison.OrdinalIgnoreCase) || row["SpaceCapacity"].ToString().Contains(search, StringComparison.OrdinalIgnoreCase))
                                     .CopyToDataTable();
            }

            int totalItems = dataTable.Rows.Count;
            var pagedData = dataTable.AsEnumerable()
                                     .Skip((page - 1) * pageSize)
                                     .Take(pageSize)
                                     .CopyToDataTable();

            foreach (System.Data.DataRow row in pagedData.Rows)
            {
                row["Filename"] = _sharedFileService.GetSharedFilePath(row["Filename"].ToString());
            }

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.SearchQuery = search;

            return View(pagedData);
        }


        // GET: SpaceController/Details/5
        public ActionResult Details(int id)
        {
            ViewData["isCustomerLoggedIn"] = !string.IsNullOrEmpty(HttpContext.Session.GetString("customerEmail"));
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

            if (beginDate > endDate)
            {
                ModelState.AddModelError("date", "La date de début ne peut pas être supérieure à la date de fin.");
                return View();
            }

            int customerID = int.Parse(HttpContext.Session.GetString("customerID"));
            int spaceID = int.Parse(space);

            var booking = new Booking()
            {
                CustomerID = customerID,
                BookingDate = beginDate,
                BookingEndDate = endDate,
                BookingPaidAmount = 0,
                BookingPrice = 0,
                SpaceID = spaceID,
                IsValidated = false,
            };

            Customer? customer = _customerService.GetUserByID(customerID);
            Space? spaceFromDB = _spaceService.GetSpaceById(spaceID);

            if (customer == null || spaceFromDB == null)
            {
                return NotFound();
            }

            string customerEmail = "nalytovo@gmail.com";

            byte[] pdfInvoice = PdfHelper.GenerateInvoice(
                "SpaceBook",
                $"{customer.CustomerName}",
                $"{customer.CustomerPhone} / {customer.CustomerEmail}",
                "---",
                DateTime.Now,
                spaceFromDB.SpaceName,
                dateDiff.Days,        // Quantité
                spaceFromDB.SpacePrice,   // Prix unitaire
                20,   // Taux de TVA en %
                booking
            );

            string subject = "Votre réservation a été créée";
            string body = $"Bonjour, votre réservation de {dateDiff.Days} jours pour l'espace {space} a été créée. " +
                          $"Date de début: {booking.BookingDate}, Date de fin: {booking.BookingEndDate}. " +
                          $"N'hésitez pas à nous contacter si besoin." +
                          $"Nous vous contacterons le plus vite plus vite possible pour la confirmation." +
                          $"À très bientôt";
            await _emailSender.SendEmailAsync(customerEmail, subject, body, pdfInvoice, "Facture_reservation.pdf");

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
