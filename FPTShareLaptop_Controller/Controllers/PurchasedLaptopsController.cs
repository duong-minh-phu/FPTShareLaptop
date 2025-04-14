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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var laptops = await _unitOfWork.PurchasedLaptop.GetAllAsync();
            var dto = _mapper.Map<IEnumerable<PurchasedLaptopDTO>>(laptops);
            return Ok(ResultModel.Success(dto));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var laptop = await _unitOfWork.PurchasedLaptop.GetByIdAsync(id);
            if (laptop == null)
                return NotFound(ResultModel.NotFound());

            var dto = _mapper.Map<PurchasedLaptopDTO>(laptop);
            return Ok(ResultModel.Success(dto));
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile? file, [FromForm] PurchasedLaptopCreateDTO dto)
        {
            if (dto == null)
                return BadRequest(ResultModel.BadRequest("Invalid data."));

            string? imageUrl = null;
            if (file != null && file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "purchased_laptops"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.Error != null)
                    return BadRequest(ResultModel.BadRequest(uploadResult.Error.Message));

                imageUrl = uploadResult.SecureUrl.ToString();
            }

            var laptop = _mapper.Map<PurchasedLaptop>(dto);
            laptop.PurchasedImageUrl = imageUrl;
            laptop.PurchaseDate = DateTime.UtcNow;
            laptop.Status = "true";

            await _unitOfWork.PurchasedLaptop.AddAsync(laptop);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<PurchasedLaptopDTO>(laptop);
            return CreatedAtAction(nameof(GetById), new { id = laptop.PurchasedLaptopId }, ResultModel.Created(result));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, IFormFile? file, [FromForm] PurchasedLaptopUpdateDTO dto)
        {
            var laptop = await _unitOfWork.PurchasedLaptop.GetByIdAsync(id);
            if (laptop == null)
                return NotFound(ResultModel.NotFound());

            if (file != null && file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "purchased_laptops"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.Error != null)
                    return BadRequest(ResultModel.BadRequest(uploadResult.Error.Message));

                laptop.PurchasedImageUrl = uploadResult.SecureUrl.ToString();
            }

            _mapper.Map(dto, laptop);
            _unitOfWork.PurchasedLaptop.Update(laptop);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var laptop = await _unitOfWork.PurchasedLaptop.GetByIdAsync(id);
            if (laptop == null)
                return NotFound(ResultModel.NotFound());

            _unitOfWork.PurchasedLaptop.Delete(laptop);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Deleted successfully"));
        }
    }
}
