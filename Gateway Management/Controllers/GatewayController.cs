using Gateway_Management.Data.DTO;
using Gateway_Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gateway_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GatewayController : ControllerBase
    {
        private readonly IGatewayService _service;

        public GatewayController(IGatewayService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Gateway>>>> GetAllGateways()
        {
            var result = await _service.GetAllGateways();
            return Ok(result);
        }
        [HttpGet("{serialNumber}")]
        public async Task<ActionResult<ServiceResponse<Gateway>>> GetGatewayBySerialNumber(string serialNumber)
        {
            var result = await _service.GetGatewayBySerialNumber(serialNumber);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<Gateway>>> CreateGateway(Gateway device)
        {
            var result = await _service.CreateGateway(device);
            return Ok(result);
        }
        [HttpPost("/peripheral-device/{gatewayId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> AddPeripheralDeviceToGateway(int gatewayId, PeripheralDevice device)
        {
            var response = await _service.AddPeripheralDeviceToGateway(gatewayId, device);
            return Ok(response);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<Gateway>>> RemoveGateway(int id)
        {
            var result = await _service.RemoveDeviceFromGateway(id);
            return Ok(result);
        } 
        [HttpDelete("/peripheraldevice/{deviceId}")]
        public async Task<ActionResult<ServiceResponse<Gateway>>> RemovePeripheralDevice(int deviceId)
        {
            var result = await _service.RemovePeripheralDevice(deviceId);
            return Ok(result);
        }
        
    }
}
