using Gateway_Management.Data.DTO;
using Gateway_Management.Data.Repository.GatewayRepository;
using Gateway_Management.Models;

namespace Gateway_Management.core.Services.GatewayServices
{
    public class GatewayService : IGatewayService
    {
        private readonly IGatewayRepository _repository;

        public GatewayService(IGatewayRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResponse<Gateway>> AddDeviceToGateway(string serialNumber, PeripheralDevice device)
        {
            return await _repository.AddDeviceToGateway(serialNumber, device);
        }

        public async Task<ServiceResponse<Gateway>> CreateGateway(Gateway gateway)
        {
            return await _repository.CreateGateway(gateway);
        }

        public async Task<ServiceResponse<List<Gateway>>> GetAllGateways()
        {
            return await _repository.GetAllGateways();
        }

        public async Task<ServiceResponse<Gateway>> GetGatewayBySerialNumber(string serialNumber)
        {
            return await _repository.GetGatewayBySerialNumber(serialNumber);
        }

        public async Task<ServiceResponse<bool>> RemoveDeviceFromGateway(int Id)
        {
            return await _repository.RemoveDeviceFromGateway(Id);
        }
    }
}
