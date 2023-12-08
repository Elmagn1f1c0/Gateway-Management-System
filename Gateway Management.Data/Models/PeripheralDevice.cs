using Gateway_Management.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gateway_Management.Models
{
    public class PeripheralDevice
    {
        [Key] 
        public int DeviceId { get; set; }
        public int UID { get; set; }
        public string Vendor { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public Status Status { get; set; }
        [ForeignKey("Gateway")]
        public int? GatewayId { get; set; }
        

    }
}
