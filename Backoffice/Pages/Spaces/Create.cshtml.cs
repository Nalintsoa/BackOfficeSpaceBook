using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Backoffice.Data;
using Backoffice.Models;
using Microsoft.Extensions.Hosting;

namespace Backoffice.Pages.Spaces
{
    public class CreateModel : PageModel
    {
        private readonly Backoffice.Data.SpaceBookContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public CreateModel(Backoffice.Data.SpaceBookContext context, IWebHostEnvironment environment, IConfiguration configuration)
        {
            _context = context;
            _environment = environment;
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Space Space { get; set; } = default!;

        [BindProperty]
        public IFormFile Filename { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Filename == null || Filename.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Please select an image to upload.");
                return Page();
            }

            // Enregistrer le fichier dans le dossier wwwroot/images
            var sharedPath = _configuration["SharedFilesPath"];
            var uploadsFolder = Path.Combine(sharedPath, "images");
            Directory.CreateDirectory(uploadsFolder); // S'assure que le dossier existe

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Filename.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await Filename.CopyToAsync(stream);
            }

            // Enregistrer le chemin dans la base de données
            var imageRecord = new Space
            {
                SpacePrice = Space.SpacePrice,
                SpaceCapacity = Space.SpaceCapacity,
                SpaceName = Space.SpaceName,
                SpaceDescription = Space.SpaceDescription,
                Filename = Path.Combine("images", uniqueFileName),
            };
            _context.Spaces.Add(imageRecord);
            

            //_context.Spaces.Add(Space);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
