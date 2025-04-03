using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObjects.Models;
using DataAccess.TrackingInfoDTO;
using Service.IService;

namespace Service.Service
{
    public class TrackingInfoService : ITrackingInfoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrackingInfoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TrackingInfoResModel>> GetTrackingByShipmentIdAsync(int shipmentId)
        {
            var trackingList = await _unitOfWork.TrackingInfo.GetAllAsync(t => t.ShipmentId == shipmentId);
            return trackingList.Select(t => new TrackingInfoResModel
            {
                TrackingId = t.TrackingId,
                Status = t.Status,
                Location = t.Location,
                UpdatedAt = t.UpdatedAt,
            }).ToList();
        }

        public async Task<TrackingInfoResModel> GetTrackingByIdAsync(int trackingId)
        {
            var tracking = await _unitOfWork.TrackingInfo.GetByIdAsync(trackingId);
            if (tracking == null) throw new Exception("Tracking info not found");

            return new TrackingInfoResModel
            {
                TrackingId = tracking.TrackingId,
                Status = tracking.Status,
                Location = tracking.Location,
                UpdatedAt = tracking.UpdatedAt
            };
        }

        public async Task CreateTrackingAsync(CreateTrackingInfoReqModel model)
        {
            var tracking = new TrackingInfo
            {
                ShipmentId = model.ShipmentId,
                Status = model.Status,
                Location = model.Location,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.TrackingInfo.AddAsync(tracking);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateTrackingAsync(int trackingId, UpdateTrackingInfoReqModel model)
        {
            var tracking = await _unitOfWork.TrackingInfo.GetByIdAsync(trackingId);
            if (tracking == null) throw new Exception("Tracking info not found");

            tracking.Status = model.Status;
            tracking.Location = model.Location;
            tracking.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.TrackingInfo.Update(tracking);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteTrackingAsync(int trackingId)
        {
            var tracking = await _unitOfWork.TrackingInfo.GetByIdAsync(trackingId);
            if (tracking == null) throw new Exception("Tracking info not found");

            _unitOfWork.TrackingInfo.Delete(tracking);
            await _unitOfWork.SaveAsync();
        }
    }
}
