using System.Diagnostics;
using Frontoffice.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Data;
using Frontoffice.Services;

namespace Frontoffice.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _connectionString;
        private readonly SharedFileService _sharedFileService;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, SharedFileService sharedFileService)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _sharedFileService = sharedFileService;
        }

        public IActionResult Index()
        {
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
                    var command = new SqlCommand("INSERT INTO Customer (CustomerName, CustomerFirstname, CustomerEmail, CustomerPassword, CustomerPhone) VALUES (@Name, @Firstname, @Email, @Password, @Phone)", connection);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Firstname", firstname);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@Phone", phone);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.ToString());
            }

            return RedirectToAction("Index");
        }
    }
}
