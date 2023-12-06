using Gateway_Management.Data.DTO;
using Gateway_Management.Models;

namespace Gateway_Management.core.Services.GatewayServices
{
    public interface IGatewayService
    {
        Task<ServiceResponse<List<Gateway>>> GetAllGateways();
        Task<ServiceResponse<Gateway>> GetGatewayBySerialNumber(string serialNumber);
        Task<ServiceResponse<Gateway>> CreateGateway(Gateway gateway);
        Task<ServiceResponse<Gateway>> AddDeviceToGateway(string serialNumber, PeripheralDevice device);
        Task<ServiceResponse<bool>> RemoveDeviceFromGateway(int Id);
    }
}
