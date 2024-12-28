using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backoffice.Data;
using Backoffice.Models;

namespace Backoffice.Pages.Spaces
{
    public class EditModel : PageModel
    {
        private readonly Backoffice.Data.SpaceBookContext _context;

        public EditModel(Backoffice.Data.SpaceBookContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Space Space { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var space =  await _context.Spaces.FirstOrDefaultAsync(m => m.SpaceID == id);
            if (space == null)
            {
                return NotFound();
            }
            Space = space;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Space).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpaceExists(Space.SpaceID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SpaceExists(int id)
        {
            return _context.Spaces.Any(e => e.SpaceID == id);
        }
    }
}
