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
        [HttpGet("serial-number")]
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
        [HttpPost("{serialNumber}/devices")]
        public IActionResult AddDeviceToGateway(string serialNumber, [FromBody] PeripheralDevice device)
        {
            var response = _service.AddDeviceToGateway(serialNumber, device);
            return Ok(response);
        }
        [HttpDelete]
        public async Task<ActionResult<ServiceResponse<Gateway>>> RemoveGateway(int id)
        {
            var result = await _service.RemoveDeviceFromGateway(id);
            return Ok(result);
        }
        
    }
}
