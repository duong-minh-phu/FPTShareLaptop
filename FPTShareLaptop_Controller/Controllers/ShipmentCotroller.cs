using System.Net;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using DataAccess.ResultModel;
using DataAccess.ShipmentDTO;
using DataAccess.TrackingInfoDTO;
using System.Threading.Tasks;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
        private readonly IShipmentService _shipmentService;

        public ShipmentController(IShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }

        [HttpGet]
        [Route("get/{shipmentId}")]
        public async Task<IActionResult> GetShipmentById(int shipmentId)
        {
            var result = await _shipmentService.GetShipmentByIdAsync(shipmentId);
            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Shipment retrieved successfully.",
                Data = result
            };
            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateShipment([FromBody] CreateShipmentReqModel request)
        {
            await _shipmentService.CreateShipmentAsync(request);
            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Shipment created successfully.",
            };
            return StatusCode(response.Code, response);
        }

        [HttpDelete]
        [Route("delete/{shipmentId}")]
        public async Task<IActionResult> DeleteShipment(int shipmentId)
        {
            await _shipmentService.DeleteShipmentAsync(shipmentId);
            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Shipment deleted successfully."
            };
            return StatusCode(response.Code, response);
        }
    }
}