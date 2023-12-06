using System.ComponentModel.DataAnnotations;

namespace Gateway_Management.Models
{
    public class PeripheralDevice
    {
        [Key] // Define the primary key
        public int Id { get; set; }
        public int UID { get; set; }
        public string Vendor { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public bool Status { get; set; }
        public int? GatewayId { get; set; }

    }
}
