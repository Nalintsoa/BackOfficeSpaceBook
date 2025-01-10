using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Humanizer;

namespace Backoffice.Models
{
    public class Booking
    {
        public int BookingID { get; set; }
        [DisplayName("Client")]
        public int CustomerID { get; set; }
        [DisplayName("Salle")]
        public int SpaceID { get; set; }
        [DisplayName("Début de réservation")]
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
        public double BookingPrice { 
            get {
                if (Space != null) {
                    return ((BookingEndDate - BookingDate).Days + 1) * Space.SpacePrice;
                } else
                {
                    return 0;
                }
            }
            set { } 
        }

        public bool? IsValidated { get; set; }

        [DisplayName("Client")]
        public Customer? Customer { get; set; }
        [DisplayName("Salle")]
        public Space? Space { get; set; }

        public ICollection<BookingEquip>? BookingEquips { get; set; }

        [DisplayName("Nombre de jours")]
        public int? DateDiff
        {
            get { return (BookingEndDate - BookingDate).Days + 1; }
            set {
                if (value.HasValue)
                {
                    BookingEndDate = BookingDate.AddDays(value.Value);
                }
            } 
        }

        public override string ToString()
        {
            return $"Booking ID: {BookingID}\n" +
                   $"Client: {Customer}\n" +
                   $"Salle: {Space?.SpaceName ?? "Space not available"}\n" +
                   $"Début de réservation: {BookingDate:dd-MM-yyyy}\n" +
                   $"Fin de réservation: {BookingEndDate:dd-MM-yyyy}\n" +
                   $"Montant payé: {BookingPaidAmount?.ToString("C") ?? "Non payé"}\n" +
                   $"Prix de la location: {BookingPrice:C}\n" +
                   $"Nombre de jours: {DateDiff}";
        }
    }
}
