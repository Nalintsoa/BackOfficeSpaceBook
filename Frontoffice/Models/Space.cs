using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Frontoffice.Models
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

        [DisplayName("Image")]
        public string? Filename { get; set; }

        //public ICollection<Booking>? Bookings { get; set; }
    }
}
