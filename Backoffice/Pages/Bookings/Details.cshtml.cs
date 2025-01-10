using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Backoffice.Data;
using Backoffice.Models;

namespace Backoffice.Pages.Bookings
{
    public class DetailsModel : PageModel
    {
        private readonly Backoffice.Data.SpaceBookContext _context;

        public DetailsModel(Backoffice.Data.SpaceBookContext context)
        {
            _context = context;
        }

        public Booking Booking { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(s => s.Customer)
                .Include(s => s.Space)
                .FirstOrDefaultAsync(m => m.BookingID == id);
            if (booking == null)
            {
                return NotFound();
            }
            else
            {
                Console.WriteLine(booking.ToString());
                Booking = booking;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var bookingId = int.Parse(Request.Form["Booking.BookingID"]);
            var booking = await _context.Bookings.FindAsync(bookingId);

            if (booking == null)
            {
                return NotFound();
            }

            booking.IsValidated = true;
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index"); 
        }
    }
}
