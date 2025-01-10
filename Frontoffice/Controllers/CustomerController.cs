using System.Data;
using Frontoffice.Models;
using Frontoffice.Services;
using Frontoffice.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Frontoffice.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly CustomerService _customerService;
        private readonly BookingService _bookingService;

        public CustomerController(ILogger<CustomerController> logger, CustomerService customerService, BookingService bookingService)
        {
            _logger = logger;
            _customerService = customerService;
            _bookingService = bookingService;
        }
        public async Task<IActionResult> Index()
        {
            var customerID = HttpContext.Session.GetString("customerID");
            var customerEmail = HttpContext.Session.GetString("customerEmail");
            if (string.IsNullOrEmpty(customerID) || string.IsNullOrEmpty(customerEmail))
            {
                return Unauthorized("L'utilisateur n'est pas connecté");
            }
            
            var customer = _customerService.GetUserByEmail(customerEmail);
            if (customer == null) {
                return Unauthorized("Utilisateur inexistant");
            }

            var bookings = await _bookingService.GetBookingsByCustomerIdAsync(customer.CustomerID);

            CustomerViewModel viewModel = new CustomerViewModel { Customer = customer, Bookings = bookings };

            return View(viewModel);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
