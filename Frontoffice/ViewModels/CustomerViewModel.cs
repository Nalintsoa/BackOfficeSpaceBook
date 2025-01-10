using Frontoffice.Models;

namespace Frontoffice.ViewModels
{
    public class CustomerViewModel
    {
        public Models.Customer Customer { get; set; }
        public IEnumerable<Booking> Bookings { get; set; }
    }
}
