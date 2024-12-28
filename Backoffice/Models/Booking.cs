using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backoffice.Models
{
    public class Booking
    {
        public int BookingID { get; set; }
        [DisplayName("Client")]
        public int CustomerID { get; set; }
        [DisplayName("Salle")]
        public int SpaceID { get; set; }
        [DisplayName("Date de réservation")]
        [Column(TypeName = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BookingDate { get; set; }
        [DisplayName("Montant payé")]
        public double? BookingPaidAmount { get; set; }
        [DisplayName("Prix de la location")]
        public double? BookingPrice { get; set; }

        public bool? IsCanceled { get; set; }



        [DisplayName("Client")]
        public Customer? Customer { get; set; }
        [DisplayName("Salle")]
        public Space? Space { get; set; }

        public ICollection<BookingEquip>? BookingEquips { get; set; }
    }
}
