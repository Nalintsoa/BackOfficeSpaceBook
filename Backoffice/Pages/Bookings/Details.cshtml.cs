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

            var booking = await _context.Bookings.FirstOrDefaultAsync(m => m.BookingID == id);
            if (booking == null)
            {
                return NotFound();
            }
            else
            {
                Booking = booking;
            }
            return Page();
        }
    }
}
