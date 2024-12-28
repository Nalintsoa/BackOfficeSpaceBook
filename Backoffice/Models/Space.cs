using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Backoffice.Models
{
    public class Space
    {
        public int SpaceID { get; set; }
        [DisplayName("Nom")]
        [Required]
        public string SpaceName { get; set; }
        [DisplayName("Capacité")]
        [Required]
        public int SpaceCapacity { get; set; }
        [DisplayName("Prix")]
        [Required]
        public double SpacePrice { get; set; }

        public ICollection<Booking>? Bookings { get; set; }
    }
}
