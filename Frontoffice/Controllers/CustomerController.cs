using System.Data;
using Frontoffice.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Frontoffice.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly CustomerService _customerService;

        public CustomerController(ILogger<CustomerController> logger, CustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }
        public IActionResult Index()
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

            return View(customer);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
