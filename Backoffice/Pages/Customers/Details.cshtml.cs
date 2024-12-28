using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Backoffice.Data;
using Backoffice.Models;

namespace Backoffice.Pages.Customers
{
    public class DetailsModel : PageModel
    {
        private readonly Backoffice.Data.SpaceBookContext _context;

        public DetailsModel(Backoffice.Data.SpaceBookContext context)
        {
            _context = context;
        }

        public Customer Customer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var customer = await _context.Customers.FirstOrDefaultAsync(m => m.CustomerID == id);
            var customer = await _context.Customers
                .Include(c => c.Bookings)
                .ThenInclude(b => b.Space)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CustomerID == id);

            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                Customer = customer;
            }
            return Page();
        }
    }
}
