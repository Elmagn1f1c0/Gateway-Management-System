using Gateway_Management.Data.DTO;
using Gateway_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway_Management.Data.Repository.GatewayRepository
{
    public interface IGatewayRepository
    {
        Task<ServiceResponse<List<Gateway>>> GetAllGateways();
        Task<ServiceResponse<Gateway>> GetGatewayBySerialNumber(string serialNumber);
        Task<ServiceResponse<Gateway>> CreateGateway(Gateway gateway);
        Task<ServiceResponse<Gateway>> AddDeviceToGateway(string serialNumber, PeripheralDevice device);
        Task<ServiceResponse<bool>> RemoveDeviceFromGateway(int Id);
    }
}
