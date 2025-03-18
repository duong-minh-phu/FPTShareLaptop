﻿using BusinessObjects.Enums;
using BusinessObjects.Models;
using DataAccess.DonationFormDTO;
using Service.IService;

namespace Service.Service
{
    public class DonationFormService : IDonateFormService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DonationFormService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DonateFormResModel>> GetAllDonationsAsync()
        {
            var donations = await _unitOfWork.DonateForm.GetAllAsync();
            return donations.Select(d => new DonateFormResModel
            {
                DonationFormId = d.DonateFormId,
                SponsorId = d.UserId,  
                ItemName = d.ItemName,
                ItemDescription = d.ItemDescription,
                Quantity = d.DonateQuantity,  
                Status = d.Status,
                CreatedAt = DateTime.UtcNow
            }).ToList();
        }

        public async Task<DonateFormResModel?> GetDonationByIdAsync(int id)
        {
            var donation = await _unitOfWork.DonateForm.GetByIdAsync(id);
            if (donation == null) return null;

            return new DonateFormResModel
            {
                DonationFormId = donation.DonateFormId,
                SponsorId = donation.UserId,
                ItemName = donation.ItemName,
                ItemDescription = donation.ItemDescription,
                Quantity = donation.DonateQuantity,
                Status = donation.Status,
                CreatedAt = DateTime.UtcNow
            };
        }


        public async Task CreateDonationAsync(CreateDonateFormReqModel request)
        {
            var newDonate = new DonateForm
            {
                ItemName = request.ItemName,
                ItemDescription = request.ItemDescription,
                DonateQuantity = request.Quantity,
                Status = DonationStatus.Pending.ToString(),
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.DonateForm.AddAsync(newDonate);
            await _unitOfWork.SaveAsync();
        }


        public async Task UpdateDonationAsync(int id, UpdateDonateFormReqModel request)
        {
            var donation = await _unitOfWork.DonateForm.GetByIdAsync(id);
            if (donation == null) throw new Exception("Donation not found");

            donation.ItemName = request.ItemName;
            donation.ItemDescription = request.ItemDescription;
            donation.DonateQuantity = request.Quantity;           

            _unitOfWork.DonateForm.Update(donation);
            await _unitOfWork.SaveAsync();
        }

        public async Task SoftDeleteDonationAsync(int id)
        {
            var donation = await _unitOfWork.DonateForm.GetByIdAsync(id);
            if (donation == null) throw new Exception("Donation not found");

            donation.Status = DonationStatus.Rejected.ToString();
            _unitOfWork.DonateForm.Update(donation);
            await _unitOfWork.SaveAsync();
        }
    }
}
