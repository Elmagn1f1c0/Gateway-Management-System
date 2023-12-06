using Gateway_Management.Data.Context;
using Gateway_Management.Data.DTO;
using Gateway_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Gateway_Management.Data.Repository.GatewayRepository
{
    public class GatewayRepository : IGatewayRepository
    {
        private readonly ApplicationDbContext _context;

        public GatewayRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<Gateway>>> GetAllGateways()
        {
            var gatewaysWithDevices = await _context.Gateways.Include(g => g.Devices).ToListAsync();

            return new ServiceResponse<List<Gateway>> { Data = gatewaysWithDevices };
        }

        public async Task<ServiceResponse<Gateway>> GetGatewayBySerialNumber(string serialNumber)
        {
            var gateway = await _context.Gateways
                .Include(g => g.Devices)
                .FirstOrDefaultAsync(g => g.SerialNumber == serialNumber);

            if (gateway == null)
            {
                return new ServiceResponse<Gateway>
                {
                    StatusCode = 400,
                    Success = false,
                    Message = "Gateway not found"
                };
            }

            return new ServiceResponse<Gateway> { Data = gateway };
        }


        public async Task<ServiceResponse<Gateway>> CreateGateway(Gateway gateway)
        {
            return await Task.Run(() =>
            {
                if (_context.Gateways.Any(g => g.SerialNumber == gateway.SerialNumber))
                {
                    return new ServiceResponse<Gateway> { Success = false, Message = "Gateway serialNumber already exists" };
                }

                if (_context.Gateways.Any(g => g.IPv4Address == gateway.IPv4Address))
                {
                    return new ServiceResponse<Gateway> { Success = false, Message = "IPv4 address already exists" };
                }

                if (!IsValidIPv4(gateway.IPv4Address))
                {
                    return new ServiceResponse<Gateway> { Success = false, Message = "Invalid IPv4 address" };
                }

                if (gateway.Devices.Count > 10)
                {
                    return new ServiceResponse<Gateway> { Success = false, Message = "Gateway cannot have more than 10 devices" };
                }

                _context.Gateways.Add(gateway);
                _context.SaveChanges();
                return new ServiceResponse<Gateway> { Data = gateway };
            });
        }

        public async Task<ServiceResponse<Gateway>> AddDeviceToGateway(string serialNumber, PeripheralDevice device)
        {
            return await Task.Run(() =>
            {
                var gateway = _context.Gateways.FirstOrDefault(g => g.SerialNumber == serialNumber);
                if (gateway == null)
                {
                    return new ServiceResponse<Gateway>
                    {
                        StatusCode = 400,
                        Success = false,
                        Message = "Gateway not found"
                    };
                }

                if (gateway.Devices.Count >= 10)
                {
                    return new ServiceResponse<Gateway> { Success = false, Message = "Gateway already has 10 devices" };
                }

                gateway.Devices.Add(device);
                return new ServiceResponse<Gateway> { Data = gateway };
            });
        }
        public async Task<ServiceResponse<bool>> RemoveDeviceFromGateway(int Id)
        {
            var device = await _context.Gateways.FindAsync(Id);

            if (device == null)
            {
                return new ServiceResponse<bool>
                {
                    StatusCode = 400,
                    Success = false,
                    Message = "Device not found"
                };
            }
            _context.Gateways.Remove(device);
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Success = true, Message = "Device removed successfully" };
        }

        private bool IsValidIPv4(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                return false;
            }
            string[] octets = ipAddress.Split('.');
            if (octets.Length != 4)
            {
                return false;
            }

            foreach (string octet in octets)
            {
                if (!byte.TryParse(octet, out byte result))
                {
                    return false;
                }

                if (result < 0 || result > 255)
                {
                    return false;
                }
            }

            return true;
        }

    }
}
