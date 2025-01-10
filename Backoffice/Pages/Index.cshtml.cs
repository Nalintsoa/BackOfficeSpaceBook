using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Backoffice.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly Backoffice.Data.SpaceBookContext _context;

        public IndexModel(ILogger<IndexModel> logger, Backoffice.Data.SpaceBookContext context)
        {
            _logger = logger;
            _context = context;
        }

        public int customerCount { get; set; } = 0;
        public int spaceCount { get; set; } = 0;
        public int invalidBookingCount {  get; set; } = 0;
        public int bookingCount { get; set; } = 0;

        public async Task<IActionResult> OnGetAsync()
        {
            customerCount = await _context.Customers.CountAsync();
            spaceCount = await _context.Spaces.CountAsync();
            invalidBookingCount = await _context.Bookings.Where(b => b.IsValidated != true).CountAsync();
            bookingCount = await _context.Bookings.CountAsync();
            return Page();
        }

        public async Task<List<ReservationCount>> GetReservationCountByMonth(DateTime month)
        {
            var firstDayOfMonth = new DateTime(month.Year, month.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var reservations = await _context.Bookings
                .Where(b => b.BookingDate >= firstDayOfMonth && b.BookingDate <= lastDayOfMonth)
                .ToListAsync();

            var reservationCounts = reservations.GroupBy(b => b.BookingDate.Day)
                .Select(g => new ReservationCount
                {
                    Day = g.Key,
                    Count = g.Count()
                })
                .OrderBy(rc => rc.Day)
                .ToList();

            return reservationCounts;
        }

        public async Task<List<KeyCount>> GetReservationBySpace()
        {
            var reservations = await _context.Bookings
                .Include(b => b.Space)
                .ToListAsync();

            var reservationCounts = reservations.GroupBy(b => b.Space)
                .Select(g => new KeyCount
                {
                    Key = g.Key.SpaceName,
                    Count = g.Count()
                })
                .OrderBy(rc => rc.Key)
                .ToList();

            return reservationCounts;
        }
    }

    

public class ReservationCount
    {
        public int Day { get; set; }
        public int Count { get; set; }
        public override string ToString() {
            return $"{Day} {Count}";
        }
    }
}

public class KeyCount
{
    public string Key { get; set; }
    public int Count { get; set; }
}

