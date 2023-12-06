using System.ComponentModel.DataAnnotations;

namespace Gateway_Management.Models
{
    public class Gateway
    {
        [Key] // Define the primary key
        public int Id { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        
        public string IPv4Address { get; set; } = string.Empty;
        public List<PeripheralDevice> Devices { get; set; } = new List<PeripheralDevice>();
    }
}
