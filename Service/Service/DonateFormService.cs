using System.Net;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using DataAccess.DonationFormDTO;
using Service.IService;
using Service.Utils.CustomException;

namespace Service.Service
{
    public class DonationFormService : IDonateFormService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Cloudinary _cloudinary;
        private readonly IJWTService _jwtService;

        public DonationFormService(IUnitOfWork unitOfWork, Cloudinary cloudinary, IJWTService jWTService)
        {
            _unitOfWork = unitOfWork;
            _cloudinary = cloudinary;
            _jwtService = jWTService;
        }

        public async Task<List<DonateFormResModel>> GetAllDonationsAsync()
        {
            var donations = await _unitOfWork.DonateForm.GetAllAsync();
            return donations.Select(d => new DonateFormResModel
            {
                DonationFormId = d.DonateFormId,
                SponsorId = d.UserId,  
                ItemName = d.ItemName,
                ItemDescription = d.ItemDescription,
                ImageDonateForm = d.ImageDonateForm,
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
                ImageDonateForm = donation.ImageDonateForm,
                Quantity = donation.DonateQuantity,
                Status = donation.Status,
                CreatedAt = DateTime.UtcNow
            };
        }


        public async Task<DonateFormResModel> CreateDonationAsync(CreateDonateFormReqModel request)
        {
            using var stream = request.ImageDonateForm.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(request.ImageDonateForm.FileName, stream),
                Folder = "form_images"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.Error != null)
            {
                throw new ApiException(HttpStatusCode.InternalServerError, uploadResult.Error.Message);
            }

            string imageFormUrl = uploadResult.SecureUrl.ToString();

            var newDonate = new DonateForm
            {

                UserId=request.SponsorId,

                ItemName = request.ItemName,
                ItemDescription = request.ItemDescription,
                DonateQuantity = request.Quantity,
                ImageDonateForm = imageFormUrl,
                Status = DonateStatus.Pending.ToString(),
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.DonateForm.AddAsync(newDonate);
            await _unitOfWork.SaveAsync();
            
            return new DonateFormResModel
            {
                DonationFormId = newDonate.DonateFormId,
                SponsorId = newDonate.UserId,
                ItemName = newDonate.ItemName,
                ItemDescription = newDonate.ItemDescription,
                ImageDonateForm = newDonate.ImageDonateForm,
                Quantity = newDonate.DonateQuantity,
                CreatedAt = newDonate.CreatedDate,
                Status = newDonate.Status
            };
        }


        public async Task UpdateDonationAsync(string token, int id, UpdateDonateFormReqModel request)
        {
            var userId = _jwtService.decodeToken(token, "userId");
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "User not found.");

            var donation = await _unitOfWork.DonateForm.GetByIdAsync(id);
            if (donation == null) throw new ApiException(HttpStatusCode.BadRequest, "Donation not found");


            donation.Status = request.Status;          

            _unitOfWork.DonateForm.Update(donation);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteDonationAsync(int id)
        {
            var donation = await _unitOfWork.DonateForm.GetByIdAsync(id);
            if (donation == null) throw new ApiException(HttpStatusCode.BadRequest, "Donation not found");
            
            _unitOfWork.DonateForm.Delete(donation);
            await _unitOfWork.SaveAsync();
        }
    }
}
