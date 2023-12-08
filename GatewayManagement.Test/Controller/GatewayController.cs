using Moq;
using Gateway_Management.Controllers;
using Gateway_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Gateway_Management.core.Services.GatewayServices;
using Gateway_Management.Data.DTO;

namespace Gateway_Management.Tests.Controllers
{
    public class GatewayControllerTests
    {
        [Fact]
        public async Task GetAllGateways_Returns_OkResult()
        {
            // Arrange
            var mockService = new Mock<IGatewayService>();
            var controller = new GatewayController(mockService.Object);

            var expectedGateways = new List<Gateway>
            {
                new Gateway { Id = 1, Name = "Gateway 1" },
                new Gateway { Id = 2, Name = "Gateway 2" }
            };

            var successfulResponse = new ServiceResponse<List<Gateway>>
            {
                StatusCode = 200,
                Data = expectedGateways
            };

            mockService.Setup(service => service.GetAllGateways()).ReturnsAsync(successfulResponse);

            // Act
            var actionResult = await controller.GetAllGateways();
            var okObjectResult = actionResult.Result as OkObjectResult;
            var resultData = okObjectResult?.Value as ServiceResponse<List<Gateway>>;

            // Assert
            Assert.NotNull(okObjectResult);
            Assert.NotNull(resultData);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal(expectedGateways, resultData.Data);
        }

        [Fact]
        public async Task GetGatewayBySerialNumber_WhenGatewayFound_Returns_OkResult()
        {
            // Arrange
            var mockService = new Mock<IGatewayService>();
            var controller = new GatewayController(mockService.Object);

            var expectedGateway = new Gateway
            {
                Id = 1,
                SerialNumber = "123456",
                Name = "Test Gateway"
            };

            var successfulResponse = new ServiceResponse<Gateway>
            {
                StatusCode = 200,
                Data = expectedGateway
            };

            mockService.Setup(service => service.GetGatewayBySerialNumber("123456")).ReturnsAsync(successfulResponse);

            // Act
            var actionResult = await controller.GetGatewayBySerialNumber("123456");
            var okObjectResult = actionResult.Result as OkObjectResult;
            var resultData = okObjectResult?.Value as ServiceResponse<Gateway>;

            // Assert
            Assert.NotNull(okObjectResult);
            Assert.NotNull(resultData);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal(expectedGateway, resultData.Data);
        }
        [Fact]
        public async Task GetGatewayBySerialNumber_WhenGatewayNotFound_Returns_OkResult()
        {
            // Arrange
            var mockService = new Mock<IGatewayService>();
            var controller = new GatewayController(mockService.Object);
            ServiceResponse<Gateway> notFoundResponse = new ServiceResponse<Gateway>
            {
                StatusCode = 200,
                Success = false,
                Message = "Gateway not found"
            };
            mockService.Setup(service => service.GetGatewayBySerialNumber("NonExistentSerial")).ReturnsAsync(notFoundResponse);

            // Act
            var actionResult = await controller.GetGatewayBySerialNumber("NonExistentSerial");
            var okResult = actionResult.Result as ObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            if (okResult.Value is ServiceResponse<Gateway> serviceResponse)
            {
                Assert.False(serviceResponse.Success);
                Assert.Equal("Gateway not found", serviceResponse.Message);
            }
            else
            {
                Assert.True(false, "Unexpected value type returned");
            }
        }

    }
}
