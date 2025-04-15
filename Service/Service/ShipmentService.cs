using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BusinessObjects.Models;
using CloudinaryDotNet;
using DataAccess.ShipmentDTO;
using Service.IService;
using Service.Utils.CustomException;

namespace Service.Service
{
    public class ShipmentService : IShipmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShipmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // ✅ Get All Shipments
        public async Task<List<ShipmentResModel>> GetAllShipmentsAsync()
        {
            var shipments = await _unitOfWork.Shipment.GetAllAsync();
            return shipments.Select(s => new ShipmentResModel
            {
                ShipmentId = s.ShipmentId,
                TrackingNumber = s.TrackingNumber,
                Carrier = s.Carrier,
                Status = s.Status,
                EstimatedDeliveryDate = s.EstimatedDeliveryDate
            }).ToList();
        }

        // ✅ Get Shipment By ID
        public async Task<ShipmentResModel> GetShipmentByIdAsync(int shipmentId)
        {
            var shipment = await _unitOfWork.Shipment.GetByIdAsync(shipmentId);
            if (shipment == null)
                throw new ApiException(HttpStatusCode.NotFound, "Shipment not found");

            return new ShipmentResModel
            {
                ShipmentId = shipment.ShipmentId,
                TrackingNumber = shipment.TrackingNumber,
                Carrier = shipment.Carrier,
                Status = shipment.Status,
                EstimatedDeliveryDate = shipment.EstimatedDeliveryDate
            };
        }

        // ✅ Update Shipment
        public async Task UpdateShipmentAsync(UpdateShipmentReqModel model, int shipmentId)
        {
            var shipment = await _unitOfWork.Shipment.GetByIdAsync(shipmentId);
            if (shipment == null)
                throw new ApiException(HttpStatusCode.NotFound, "Shipment not found");

            shipment.Status = model.Status;
            shipment.Carrier = model.Carrier;
            shipment.ShippingCost = model.ShippingCost;

            _unitOfWork.Shipment.Update(shipment);
            await _unitOfWork.SaveAsync();
        }

        // ✅ Delete Shipment
        public async Task DeleteShipmentAsync(int shipmentId)
        {
            var shipment = await _unitOfWork.Shipment.GetByIdAsync(shipmentId);
            if (shipment == null)
                throw new KeyNotFoundException("Shipment not found");

            _unitOfWork.Shipment.Delete(shipment);
            await _unitOfWork.SaveAsync();
        }

        public async Task CreateShipmentAsync(CreateShipmentReqModel model)
        {
            var shipment = new Shipment
            {
                OrderId = model.OrderId,
                TrackingNumber = model.TrackingNumber,
                Carrier = model.Carrier,
                Status = "Pending",
                EstimatedDeliveryDate = model.EstimatedDeliveryDate,
                ShippingCost = model.ShippingCost,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Shipment.AddAsync(shipment);
            await _unitOfWork.SaveAsync();
        }

    }
}
