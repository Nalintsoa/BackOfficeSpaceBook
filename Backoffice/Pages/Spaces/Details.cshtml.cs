using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Backoffice.Data;
using Backoffice.Models;

namespace Backoffice.Pages.Spaces
{
    public class DetailsModel : PageModel
    {
        private readonly Backoffice.Data.SpaceBookContext _context;

        public DetailsModel(Backoffice.Data.SpaceBookContext context)
        {
            _context = context;
        }

        public Space Space { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var space = await _context.Spaces.FirstOrDefaultAsync(m => m.SpaceID == id);
            if (space == null)
            {
                return NotFound();
            }
            else
            {
                Space = space;
            }
            return Page();
        }
    }
}
