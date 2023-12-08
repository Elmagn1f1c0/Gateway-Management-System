using Gateway_Management.Data.Context;
using Gateway_Management.Data.DTO;
using Gateway_Management.Data.Enum;
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

            return new ServiceResponse<List<Gateway>> 
            { 
                StatusCode = 200,
                Data = gatewaysWithDevices 
            };
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
                    return new ServiceResponse<Gateway> 
                    {
                        StatusCode = 400,
                        Success = false,
                        Message = "Gateway serialNumber already exists"
                    };
                }

                if (_context.Gateways.Any(g => g.IPv4Address == gateway.IPv4Address))
                {
                    return new ServiceResponse<Gateway> 
                    { 
                        StatusCode = 400,
                        Success = false, 
                        Message = "IPv4 address already exists" 
                    };
                }

                if (!IsValidIPv4(gateway.IPv4Address))
                {
                    return new ServiceResponse<Gateway> 
                    { 
                        StatusCode = 400,
                        Success = false, 
                        Message = "Invalid IPv4 address"
                    };
                }

                if (gateway.Devices.Count > 10)
                {
                    return new ServiceResponse<Gateway> 
                    { 
                        StatusCode = 400,
                        Success = false, 
                        Message = "Gateway cannot have more than 10 devices" 
                    };
                }
                foreach (var device in gateway.Devices)
                {
                    if (device.Status != Status.Online && device.Status != Status.Offline)
                    {
                        return new ServiceResponse<Gateway>
                        {
                            StatusCode = 400,
                            Success = false,
                            Message = "Device status should be either Online or Offline"
                        };
                    }
                }

                _context.Gateways.Add(gateway);
                _context.SaveChanges();
                return new ServiceResponse<Gateway> 
                { 
                    StatusCode = 200,
                    Data = gateway 
                };
            });
        }
        public async Task<ServiceResponse<bool>> AddPeripheralDeviceToGateway(int gatewayId, PeripheralDevice device)
        {
            var gateway = await _context.Gateways.Include(g => g.Devices).FirstOrDefaultAsync(g => g.Id == gatewayId);

            if (gateway == null)
            {
                return new ServiceResponse<bool>
                {
                    StatusCode = 404,
                    Success = false,
                    Message = "Gateway not found"
                };
            }

            if (gateway.Devices.Count >= 10)
            {
                return new ServiceResponse<bool>
                {
                    StatusCode = 400,
                    Success = false,
                    Message = "Gateway already has 10 devices, cannot add more"
                };
            }

            if (gateway.Devices.Count + 1 > 10)
            {
                return new ServiceResponse<bool>
                {
                    StatusCode = 400,
                    Success = false,
                    Message = "Adding this device would exceed the limit of 10 devices for the gateway"
                };
            }

            device.GatewayId = gatewayId;
            gateway.Devices.Add(device);

            await _context.SaveChangesAsync();
            gateway = await _context.Gateways.Include(g => g.Devices).FirstOrDefaultAsync(g => g.Id == gatewayId);

            if (gateway.Devices.Count > 10)
            {
                gateway.Devices.Remove(device);
                await _context.SaveChangesAsync();

                return new ServiceResponse<bool>
                {
                    StatusCode = 400,
                    Success = false,
                    Message = "Device count exceeded 10 after addition. Device not added."
                };
            }

            return new ServiceResponse<bool>
            {
                Data = true,
                StatusCode = 200,
                Success = true,
                Message = "Peripheral device added to the specified gateway successfully"
            };
        }
        //public async Task<ServiceResponse<bool>> RemoveDeviceFromGateway(int Id)
        //{
        //    var device = await _context.Gateways.FindAsync(Id);

        //    if (device == null)
        //    {
        //        return new ServiceResponse<bool>
        //        {
        //            StatusCode = 400,
        //            Success = false,
        //            Message = "Gateway not found"
        //        };
        //    }
        //    _context.Gateways.Remove(device);
        //    await _context.SaveChangesAsync();

        //    return new ServiceResponse<bool> 
        //    { 
        //        StatusCode = 200,
        //        Success = true, 
        //        Message = "Gateway removed successfully" 
        //    };
        //}
        public async Task<ServiceResponse<bool>> RemoveDeviceFromGateway(int Id)
        {
            var gateway = await _context.Gateways.Include(g => g.Devices).FirstOrDefaultAsync(g => g.Id == Id);

            if (gateway == null)
            {
                return new ServiceResponse<bool>
                {
                    StatusCode = 404,
                    Success = false,
                    Message = "Gateway not found"
                };
            }

            _context.PeripheralDevices.RemoveRange(gateway.Devices); // Remove associated devices

            _context.Gateways.Remove(gateway); // Remove the gateway itself
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool>
            {
                StatusCode = 200,
                Success = true,
                Message = "Gateway and all associated devices removed successfully"
            };
        }

        public async Task<ServiceResponse<bool>> RemovePeripheralDevice(int deviceId)
        {
            var device = await _context.PeripheralDevices.FindAsync(deviceId);

            if (device == null)
            {
                return new ServiceResponse<bool>
                {
                    StatusCode = 400,
                    Success = false,
                    Message = "Peripheral device not found"
                };
            }
            device.GatewayId = null;

            _context.PeripheralDevices.Remove(device);
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> 
            { 
                Data = true,
                StatusCode = 200,
                Success = true, 
                Message = "Peripheral device removed successfully" 
            };
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
