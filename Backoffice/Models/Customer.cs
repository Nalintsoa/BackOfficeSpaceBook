using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Backoffice.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        [DisplayName("Nom")]
        [Required]
        public string CustomerName { get; set; }
        [DisplayName("Prénoms")]
        public string? CustomerFirstname { get; set; }
        [DisplayName("Email")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string CustomerEmail { get; set; }
        [Required]
        [DisplayName("Mot de passe")]
        [DataType(DataType.Password)]
        public string CustomerPassword { get; set; }
        [DisplayName("Téléphone")]
        public string CustomerPhone { get; set; }
        public string? Salt { get; set; }

        public ICollection<Booking>? Bookings { get; set; }
    }
}
