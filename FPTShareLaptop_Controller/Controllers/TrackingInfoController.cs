using System.Net;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using DataAccess.ResultModel;
using DataAccess.TrackingInfoDTO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingInfoController : ControllerBase
    {
        private readonly ITrackingInfoService _trackingInfoService;

        public TrackingInfoController(ITrackingInfoService trackingInfoService)
        {
            _trackingInfoService = trackingInfoService;
        }

        // Tạo tracking mới
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateTracking([FromBody] CreateTrackingInfoReqModel request)
        {
            await _trackingInfoService.CreateTrackingAsync(request);
            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Tracking info created successfully.",
            };
            return StatusCode(response.Code, response);
        }

        // Lấy danh sách tracking theo ShipmentId
        [HttpGet]
        [Route("shipment/{shipmentId}")]
        public async Task<IActionResult> GetTrackingByShipmentId(int shipmentId)
        {
            var result = await _trackingInfoService.GetTrackingByShipmentIdAsync(shipmentId);
            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Tracking info retrieved successfully.",
                Data = result
            };
            return StatusCode(response.Code, response);
        }

        // Cập nhật trạng thái của tracking
        [HttpPut]
        [Route("update/{trackingId}")]
        public async Task<IActionResult> UpdateTracking(int trackingId, [FromBody] UpdateTrackingInfoReqModel request)
        {
            _trackingInfoService.UpdateTrackingAsync(trackingId, request);
            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Tracking info updated successfully.",
            };
            return StatusCode(response.Code, response);
        }

        // Xóa tracking
        [HttpDelete]
        [Route("delete/{trackingId}")]
        public async Task<IActionResult> DeleteTracking(int trackingId)
        {
            await _trackingInfoService.DeleteTrackingAsync(trackingId);
            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Tracking info deleted successfully."
            };
            return StatusCode(response.Code, response);
        }
    }
}
