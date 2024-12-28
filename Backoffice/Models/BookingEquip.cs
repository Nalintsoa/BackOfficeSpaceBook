namespace Backoffice.Models
{
    public class BookingEquip
    {
        public int BookingEquipID { get; set; }
        public int BookingID { get; set; }
        public int EquipmentID { get; set; }

        public Booking? Booking { get; set; }
        public Equipment? Equipment { get; set; }
    }
}
