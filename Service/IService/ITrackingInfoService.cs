using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.TrackingInfoDTO;

namespace Service.IService
{
    public interface ITrackingInfoService
    {
        Task<List<TrackingInfoResModel>> GetTrackingByShipmentIdAsync(int shipmentId);
        Task<TrackingInfoResModel> GetTrackingByIdAsync(int trackingId);
        Task CreateTrackingAsync(CreateTrackingInfoReqModel model);
        Task UpdateTrackingAsync(int trackingId,UpdateTrackingInfoReqModel model);
        Task DeleteTrackingAsync(int trackingId);
    }
}
