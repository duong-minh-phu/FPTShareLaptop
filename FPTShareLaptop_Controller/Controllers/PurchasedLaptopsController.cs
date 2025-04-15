using AutoMapper;
using BusinessObjects.Models;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using DataAccess.PurchasedLaptopDTO;
using DataAccess.ResultModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/purchased-laptops")]
    [ApiController]
    public class PurchasedLaptopsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;

        public PurchasedLaptopsController(IUnitOfWork unitOfWork, IMapper mapper, Cloudinary cloudinary)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinary = cloudinary;
        }

        // GET: api/purchased-laptops
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var purchasedLaptops = await _unitOfWork.PurchasedLaptop.GetAllAsync();
            var purchasedLaptopDTOs = _mapper.Map<IEnumerable<PurchasedLaptopReadDTO>>(purchasedLaptops);
            return Ok(ResultModel.Success(purchasedLaptopDTOs));
        }

        // GET: api/purchased-laptops/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var purchasedLaptop = await _unitOfWork.PurchasedLaptop.GetByIdAsync(id);
            if (purchasedLaptop == null)
                return NotFound(ResultModel.NotFound());

            var purchasedLaptopDTO = _mapper.Map<PurchasedLaptopReadDTO>(purchasedLaptop);
            return Ok(ResultModel.Success(purchasedLaptopDTO));
        }

        // POST: api/purchased-laptops
        [HttpPost]
        public async Task<IActionResult> Create(IFormFile? file, [FromForm] PurchasedLaptopCreateDTO dto)
        {
            if (dto == null)
                return BadRequest(ResultModel.BadRequest("Invalid data."));

            string? invoiceImageUrl = null;
            if (file != null && file.Length > 0)
            {
                // Upload image to Cloudinary
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "purchased_laptops"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.Error != null)
                    return BadRequest(ResultModel.BadRequest(uploadResult.Error.Message));

                invoiceImageUrl = uploadResult.SecureUrl.ToString();  // Get URL of uploaded image
            }

            // Map DTO to entity and set image URL
            var purchasedLaptop = _mapper.Map<PurchasedLaptop>(dto);
            purchasedLaptop.InvoiceImageUrl = invoiceImageUrl;
            purchasedLaptop.PurchasedDate = DateTime.UtcNow;  // Set current date for purchased date

            // Add to the database
            await _unitOfWork.PurchasedLaptop.AddAsync(purchasedLaptop);
            await _unitOfWork.SaveAsync();

            // Return result
            var result = _mapper.Map<PurchasedLaptopReadDTO>(purchasedLaptop);
            return CreatedAtAction(nameof(GetById), new { id = purchasedLaptop.PurchasedLaptopId }, ResultModel.Created(result));
        }

        // PUT: api/purchased-laptops/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, IFormFile? file, [FromForm] PurchasedLaptopUpdateDTO dto)
        {
            if (dto == null)
                return BadRequest(ResultModel.BadRequest("Invalid data."));

            var purchasedLaptop = await _unitOfWork.PurchasedLaptop.GetByIdAsync(id);
            if (purchasedLaptop == null)
                return NotFound(ResultModel.NotFound());

            if (file != null && file.Length > 0)
            {
                // Upload image to Cloudinary
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "purchased_laptops"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.Error != null)
                    return BadRequest(ResultModel.BadRequest(uploadResult.Error.Message));

                purchasedLaptop.InvoiceImageUrl = uploadResult.SecureUrl.ToString();  // Set new image URL
            }

            // Map updated fields to the existing purchased laptop
            _mapper.Map(dto, purchasedLaptop);

            // Update database
            _unitOfWork.PurchasedLaptop.Update(purchasedLaptop);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Updated successfully"));
        }

        // DELETE: api/purchased-laptops/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var purchasedLaptop = await _unitOfWork.PurchasedLaptop.GetByIdAsync(id);
            if (purchasedLaptop == null)
                return NotFound(ResultModel.NotFound());

            // Delete the purchased laptop from the database
            _unitOfWork.PurchasedLaptop.Delete(purchasedLaptop);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Deleted successfully"));
        }
    }

}
