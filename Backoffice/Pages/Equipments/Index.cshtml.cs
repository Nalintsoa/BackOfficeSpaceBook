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

namespace Backoffice.Pages.Equipments
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly Backoffice.Data.SpaceBookContext _context;

        public IndexModel(Backoffice.Data.SpaceBookContext context)
        {
            _context = context;
        }

        public IList<Equipment> Equipment { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Equipment = await _context.Equipments.ToListAsync();
        }
    }
}
