using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using DataAccess.ShipmentDTO;
using DataAccess.TrackingInfoDTO;

namespace Service.IService
{
    public interface IShipmentService
    {
        Task<List<ShipmentResModel>> GetAllShipmentsAsync();
        Task<ShipmentResModel> GetShipmentByIdAsync(int shipmentId);
        Task CreateShipmentAsync(CreateShipmentReqModel model);
        Task UpdateShipmentAsync(UpdateShipmentReqModel shipment, int shipmentId);
        Task DeleteShipmentAsync(int shipmentId);
    }
}
