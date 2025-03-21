using AutoMapper;
using BusinessObjects.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DataAccess.DonateItemDTO;
using DataAccess.ResultModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using Service.Service;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/donate-items")]
    [ApiController]
    public class DonateItemsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;

        public DonateItemsController(IUnitOfWork unitOfWork, IMapper mapper, Cloudinary cloudinary)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinary = cloudinary;
        }

        // GET: api/donate-items
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _unitOfWork.DonateItem.GetAllAsync();
            var itemDTOs = _mapper.Map<IEnumerable<DonateItemDTO>>(items);
            return Ok(ResultModel.Success(itemDTOs));
        }

        // GET: api/donate-items/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _unitOfWork.DonateItem.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound(ResultModel.NotFound());
            }
            var itemDTO = _mapper.Map<DonateItemDTO>(item);
            return Ok(ResultModel.Success(itemDTO));
        }

        // POST: api/donate-items (Tạo mới + upload ảnh)
        [HttpPost]
        public async Task<IActionResult> Create(IFormFile file, [FromForm] DonateItemCreateDTO itemDTO)
        {
            if (itemDTO == null)
            {
                return BadRequest(ResultModel.BadRequest("Invalid data."));
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
                    return BadRequest(ResultModel.BadRequest(uploadResult.Error.Message));

                imageUrl = uploadResult.SecureUrl.ToString();
            }

            var item = _mapper.Map<DonateItem>(itemDTO);
            item.ItemImage = imageUrl;
            item.Status = "Available";
            item.CreatedDate = DateTime.UtcNow;
            item.UpdatedDate = DateTime.UtcNow;

            await _unitOfWork.DonateItem.AddAsync(item);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<DonateItemDTO>(item);
            return CreatedAtAction(nameof(GetById), new { id = item.ItemId }, ResultModel.Created(result));
        }

        // PUT: api/donate-items/{id} (Cập nhật thông tin + upload ảnh mới)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, IFormFile? file, [FromForm] DonateItemUpdateDTO itemDTO)
        {
            if (itemDTO == null || itemDTO.ItemId != id)
            {
                return BadRequest(ResultModel.BadRequest("ID mismatch."));
            }

            var existingItem = await _unitOfWork.DonateItem.GetByIdAsync(id);
            if (existingItem == null)
            {
                return NotFound(ResultModel.NotFound());
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
                    return BadRequest(ResultModel.BadRequest(uploadResult.Error.Message));

                existingItem.ItemImage = uploadResult.SecureUrl.ToString();
            }

            _mapper.Map(itemDTO, existingItem);
            existingItem.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.DonateItem.Update(existingItem);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Updated successfully"));
        }

        // DELETE: api/donate-items/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _unitOfWork.DonateItem.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound(ResultModel.NotFound());
            }

            _unitOfWork.DonateItem.Delete(item);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Deleted successfully"));
        }
    }
}
