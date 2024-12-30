using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Frontoffice.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
