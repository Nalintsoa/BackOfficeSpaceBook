using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Backoffice.Models
{
    public class Equipment
    {
        public int EquipmentID { get; set; }
        [DisplayName("Désignation")]
        [Required]
        public string EquipmentDesignation { get; set; }
        [DisplayName("Disponible")]
        [Required]
        public int EquipmentAvailable { get; set; }
        [DisplayName("Total en stock")]
        [Required]
        public int EquipmentInStock { get; set; }
        [DisplayName("Prix unitaire")]
        [Required]
        public double EquipmentPrice { get; set; }

        public ICollection<BookingEquip>? BookingEquips { get; set; }
    }
}
