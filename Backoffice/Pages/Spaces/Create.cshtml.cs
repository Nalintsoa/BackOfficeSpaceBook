using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Backoffice.Data;
using Backoffice.Models;

namespace Backoffice.Pages.Spaces
{
    public class CreateModel : PageModel
    {
        private readonly Backoffice.Data.SpaceBookContext _context;

        public CreateModel(Backoffice.Data.SpaceBookContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Space Space { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Spaces.Add(Space);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
