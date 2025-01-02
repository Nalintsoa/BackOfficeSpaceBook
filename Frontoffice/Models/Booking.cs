using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Frontoffice.Models
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

        [DisplayName("Fin de réservation")]
        [Column(TypeName = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BookingEndDate { get; set; }

        [DisplayName("Montant payé")]
        public double? BookingPaidAmount { get; set; }
        [DisplayName("Prix de la location")]
        public double? BookingPrice { get; set; }

        public bool? IsCanceled { get; set; }



        [DisplayName("Client")]
        public Customer? Customer { get; set; }
        [DisplayName("Salle")]
        public Space? Space { get; set; }

        public override string ToString()
        {
            return $"Booking [CustomerID={CustomerID}, BookingDate={BookingDate}, BookingPaidAmount={BookingPaidAmount}, BookingPrice={BookingPrice}, SpaceID={SpaceID}, IsCanceled={IsCanceled}]";
        }


        //public ICollection<BookingEquip>? BookingEquips { get; set; }
    }
}
