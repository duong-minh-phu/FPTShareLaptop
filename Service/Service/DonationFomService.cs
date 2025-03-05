using BusinessObjects.Enums;
using BusinessObjects.Models;
using DataAccess.DonationFormDTO;
using Service.IService;

namespace Service.Service
{
    public class DonationFormService : IDonationFormService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DonationFormService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DonationFormResModel>> GetAllDonationsAsync()
        {
            var donations = await _unitOfWork.DonationForm.GetAllAsync();
            return donations.Select(d => new DonationFormResModel
            {
                DonationFormId = d.DonationFormId,
                SponsorId = d.SponsorId ?? 0,
                ItemName = d.ItemName ?? string.Empty,
                ItemDescription = d.ItemDescription ?? string.Empty,
                Quantity = d.Quantity ?? 0,
                Status = d.Status,
                CreatedAt = d.CreatedAt ?? DateTime.UtcNow
            }).ToList();
        }

        public async Task<DonationFormResModel?> GetDonationByIdAsync(int id)
        {
            var donation = await _unitOfWork.DonationForm.GetByIdAsync(id);
            if (donation == null) return null;

            return new DonationFormResModel
            {
                DonationFormId = donation.DonationFormId,
                SponsorId = donation.SponsorId ?? 0,
                ItemName = donation.ItemName ?? string.Empty,
                ItemDescription = donation.ItemDescription ?? string.Empty,
                Quantity = donation.Quantity ?? 0,
                Status = donation.Status,
                CreatedAt = donation.CreatedAt ?? DateTime.UtcNow
            };
        }

        public async Task CreateDonationAsync(CreateDonationFormReqModel request)
        {
            var newDonation = new DonationForm
            {
                ItemName = request.ItemName,
                ItemDescription = request.ItemDescription,
                Quantity = request.Quantity,
                Status = DonationStatus.Pending.ToString(),
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.DonationForm.AddAsync(newDonation);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateDonationAsync(int id, UpdateDonationFormReqModel request)
        {
            var donation = await _unitOfWork.DonationForm.GetByIdAsync(id);
            if (donation == null) throw new Exception("Donation not found");

            donation.ItemName = request.ItemName ?? donation.ItemName;
            donation.ItemDescription = request.ItemDescription ?? donation.ItemDescription;
            donation.Quantity = request.Quantity ?? donation.Quantity;           

            _unitOfWork.DonationForm.Update(donation);
            await _unitOfWork.SaveAsync();
        }

        public async Task SoftDeleteDonationAsync(int id)
        {
            var donation = await _unitOfWork.DonationForm.GetByIdAsync(id);
            if (donation == null) throw new Exception("Donation not found");

            donation.Status = DonationStatus.Rejected.ToString();
            _unitOfWork.DonationForm.Update(donation);
            await _unitOfWork.SaveAsync();
        }
    }
}
