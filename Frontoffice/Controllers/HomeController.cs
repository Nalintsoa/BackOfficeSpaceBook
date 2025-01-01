using System.Diagnostics;
using Frontoffice.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Data;
using Frontoffice.Services;
using Frontoffice.Helpers;
using Microsoft.IdentityModel.Tokens;

namespace Frontoffice.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _connectionString;
        private readonly SharedFileService _sharedFileService;
        private readonly CustomerService _customerService;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, SharedFileService sharedFileService, CustomerService customerService)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _sharedFileService = sharedFileService;
            _customerService = customerService;
        }

        public IActionResult Index()
        {
            ViewData["isCustomerLoggedIn"] = !string.IsNullOrEmpty(HttpContext.Session.GetString("customerEmail"));
            ViewData["customerfirstName"] = HttpContext.Session.GetString("customerFirstname") ?? "";
            var dataTable = new DataTable();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Space", connection);
                var adapter = new SqlDataAdapter(command);

                adapter.Fill(dataTable);
            }
            foreach (System.Data.DataRow row in dataTable.Rows) {
                row["Filename"] = _sharedFileService.GetSharedFilePath(row["Filename"].ToString());
            }
            return View(dataTable);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AddNewCustomer(string name, string firstname, string email, string password, string phone)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var salt = PasswordHelper.GenerateSalt();
                    var hashedPassword = PasswordHelper.HashPassword(password, salt);

                    var command = new SqlCommand("INSERT INTO Customer (CustomerName, CustomerFirstname, CustomerEmail, CustomerPassword, CustomerPhone, Salt) VALUES (@Name, @Firstname, @Email, @Password, @Phone, @Salt)", connection);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Firstname", firstname);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", hashedPassword);
                    command.Parameters.AddWithValue("@Phone", phone);
                    command.Parameters.AddWithValue("@Salt", salt);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.ToString());
            }

            return RedirectToAction("Index");
        }

        public IActionResult CustomerLogin(string email, string password) {
            var customer = _customerService.GetUserByEmail(email);
            if (customer == null || PasswordHelper.HashPassword(password, customer.Salt) != customer.CustomerPassword)
            {
                return Unauthorized("Invalid username or password.");
            }

            HttpContext.Session.SetString("customerID", customer.CustomerID.ToString());
            HttpContext.Session.SetString("customerEmail", customer.CustomerEmail);
            HttpContext.Session.SetString("customerFirstname", customer.CustomerFirstname ?? customer.CustomerName);

            ViewData["isCustomerLoggedIn"] = !string.IsNullOrEmpty(HttpContext.Session.GetString("customerEmail"));

            return RedirectToAction("Index", "Customer");
        }
    }
}
