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

namespace Backoffice.Pages.Spaces
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly Backoffice.Data.SpaceBookContext _context;
        private readonly Backoffice.Services.SharedFileService _sharedFileService;

        public IndexModel(Backoffice.Data.SpaceBookContext context, Backoffice.Services.SharedFileService sharedFileService)
        {
            _context = context;
            _sharedFileService = sharedFileService;
        }

        public IList<Space> Space { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var spacesList = await _context.Spaces.ToListAsync();
            foreach (var space in spacesList) {
                space.Filename = _sharedFileService.GetSharedFilePath(space.Filename);
            }
            Space = spacesList;
        }
    }
}
