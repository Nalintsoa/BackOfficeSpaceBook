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
    public class IndexModel : PageModel
    {
        private readonly Backoffice.Data.SpaceBookContext _context;

        public IndexModel(Backoffice.Data.SpaceBookContext context)
        {
            _context = context;
        }

        public IList<Space> Space { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Space = await _context.Spaces.ToListAsync();
        }
    }
}
