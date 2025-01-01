using System.Security.Claims;
using Backoffice.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Backoffice.Pages.Login
{
    public class LoginModel : PageModel
    {
        private readonly SpaceBookContext _spaceBookContext;

        public LoginModel (SpaceBookContext context)
        {
            _spaceBookContext = context;
        }

        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var user = _spaceBookContext.Users.SingleOrDefault(u => u.Username == Username);
            Console.WriteLine("user", user);
            Console.WriteLine("password", Password);
            if (user != null && VerifyPassword(Password, user.Password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToPage("/Index");
            } else {
                ModelState.AddModelError("", "Invalid login attempt.");
                return Page();
            }
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            return password == storedHash;
        }

        public void OnLogoutButtonClicked()
        {
            Console.WriteLine("ni cliquer teto izy");
        }
    }
}
