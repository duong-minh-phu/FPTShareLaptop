using AutoMapper;
using BusinessObjects.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DataAccess.DonateItemDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using Service.Service;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonateItemController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;

        public DonateItemController(IUnitOfWork unitOfWork, IMapper mapper, Cloudinary cloudinary)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinary = cloudinary;
        }

        // GET: api/DonateItem
        [HttpGet]
        public async Task<IActionResult> GetDonateItems()
        {
            var items = await _unitOfWork.DonateItem.GetAllAsync();
            var itemDTOs = _mapper.Map<IEnumerable<DonateItemDTO>>(items);
            return Ok(itemDTOs);
        }

        // GET: api/DonateItem/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDonateItem(int id)
        {
            var item = await _unitOfWork.DonateItem.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            var itemDTO = _mapper.Map<DonateItemDTO>(item);
            return Ok(itemDTO);
        }

        // POST: api/DonateItem (Tạo mới + upload ảnh)
        [HttpPost]
        public async Task<IActionResult> CreateDonateItem(IFormFile file, [FromForm] DonateItemCreateDTO itemDTO)
        {
            if (itemDTO == null)
            {
                return BadRequest("Invalid data.");
            }

            // Upload ảnh lên Cloudinary
            string imageUrl = null;
            if (file != null && file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "donate_item_images"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.Error != null)
                    return BadRequest(uploadResult.Error.Message);

                imageUrl = uploadResult.SecureUrl.ToString();
            }

            var item = _mapper.Map<DonateItem>(itemDTO);
            item.ItemImage = imageUrl; // Lưu URL ảnh vào DB
            item.Status = "Available";
            item.CreatedDate = DateTime.UtcNow;
            item.UpdatedDate = DateTime.UtcNow;

            await _unitOfWork.DonateItem.AddAsync(item);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetDonateItem), new { id = item.ItemId }, _mapper.Map<DonateItemDTO>(item));
        }

        // PUT: api/DonateItem/{id} (Cập nhật thông tin + upload ảnh mới)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDonateItem(int id, IFormFile? file, [FromForm] DonateItemUpdateDTO itemDTO)
        {
            if (itemDTO == null || itemDTO.ItemId != id)
            {
                return BadRequest("ID mismatch.");
            }

            var existingItem = await _unitOfWork.DonateItem.GetByIdAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }

            // Upload ảnh mới lên Cloudinary nếu có file mới
            if (file != null && file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "donate_item_images"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.Error != null)
                    return BadRequest(uploadResult.Error.Message);

                existingItem.ItemImage = uploadResult.SecureUrl.ToString(); // Cập nhật URL ảnh mới
            }

            _mapper.Map(itemDTO, existingItem);
            existingItem.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.DonateItem.Update(existingItem);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        // DELETE: api/DonateItem/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonateItem(int id)
        {
            var item = await _unitOfWork.DonateItem.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _unitOfWork.DonateItem.Delete(item);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
