using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Backoffice.Data;
using Backoffice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Backoffice.Pages.Bookings
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly Backoffice.Data.SpaceBookContext _context;

        public IndexModel(Backoffice.Data.SpaceBookContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; }

        public IList<Booking> Booking { get;set; } = default!;

        public async Task OnGetAsync(string sortOrder)
        {
            var bookings = _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Space)
                .AsQueryable();

            bookings = SortOrder switch
            {
                "date_desc" => bookings.OrderByDescending(b => b.BookingDate),
                "paid" => bookings.OrderBy(b => b.BookingPaidAmount),
                "paid_desc" => bookings.OrderByDescending(b => b.BookingPaidAmount),
                "price" => bookings.OrderBy(b => b.BookingPrice),
                "price_desc" => bookings.OrderByDescending(b => b.BookingPrice),
                "customer" => bookings.OrderBy(b => b.Customer.CustomerEmail),
                "customer_desc" => bookings.OrderByDescending(b => b.Customer.CustomerEmail),
                _ => bookings.OrderBy(b => b.BookingDate),
            };

            Booking = await bookings.ToListAsync();
        }
    }
}
