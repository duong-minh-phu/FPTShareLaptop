using AutoMapper;
using BusinessObjects.Models;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using DataAccess.ItemImageDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using Service.Service;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemImageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Cloudinary _cloudinary;
        private readonly IMapper _mapper;

        public ItemImageController(IUnitOfWork unitOfWork, IMapper mapper, Cloudinary cloudinary)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinary = cloudinary;
        }

        // POST: api/ItemImage (Upload ảnh mới)
        [HttpPost]
        public async Task<IActionResult> AddItemImage(IFormFile file, int itemId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file.");

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "item_images"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
                return BadRequest(uploadResult.Error.Message);

            var itemImage = new ItemImage
            {
                ItemId = itemId,
                ImageUrl = uploadResult.SecureUrl.ToString(),
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.ItemImage.AddAsync(itemImage);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetItemImagesByItemId), new { itemId }, itemImage);
        }

        // GET: api/ItemImage/{itemId} (Lấy danh sách ảnh theo ItemId)
        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetItemImagesByItemId(int itemId)
        {
            var images = await _unitOfWork.ItemImage.GetAllAsync(ii => ii.ItemId == itemId);
            var imageDtos = _mapper.Map<IEnumerable<ItemImageDTO>>(images);
            return Ok(imageDtos);
        }

        // GET: api/ItemImage (Lấy toàn bộ danh sách ảnh)
        [HttpGet]
        public async Task<IActionResult> GetAllItemImages()
        {
            var itemImages = await _unitOfWork.ItemImage.GetAllAsync();
            var itemImageDtos = _mapper.Map<IEnumerable<ItemImageDTO>>(itemImages);
            return Ok(itemImageDtos);
        }

        // PUT: api/ItemImage/{id} (Cập nhật ảnh mới)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItemImage(int id, IFormFile file)
        {
            var itemImage = await _unitOfWork.ItemImage.GetByIdAsync(id);
            if (itemImage == null)
            {
                return NotFound(new { message = $"Không tìm thấy ItemImage với Id = {id}" });
            }

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "item_images"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
                return BadRequest(uploadResult.Error.Message);

            itemImage.ImageUrl = uploadResult.SecureUrl.ToString();
            itemImage.CreatedDate = DateTime.UtcNow;

            _unitOfWork.ItemImage.Update(itemImage);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Cập nhật thành công", itemImage });
        }

        // DELETE: api/ItemImage/{id} (Xóa ảnh)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemImage(int id)
        {
            var itemImage = await _unitOfWork.ItemImage.GetByIdAsync(id);
            if (itemImage == null)
            {
                return NotFound(new { message = $"Không tìm thấy ItemImage với Id = {id}" });
            }

            _unitOfWork.ItemImage.Delete(itemImage);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Xóa thành công", itemImage });
        }
    }
}
