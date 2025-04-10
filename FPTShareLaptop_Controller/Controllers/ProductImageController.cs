using AutoMapper;
using BusinessObjects.Models;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using DataAccess.ProductImageDTO;
using DataAccess.ResultModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using Service.Service;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/product-images")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Cloudinary _cloudinary;
        private readonly IMapper _mapper;

        public ProductImageController(IUnitOfWork unitOfWork, IMapper mapper, Cloudinary cloudinary)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinary = cloudinary;
        }

        // POST: api/product-images (Upload ảnh mới)
        [HttpPost]
        public async Task<IActionResult> CreateProductImage(IFormFile file, int productId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file.");

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "product_images"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.Error != null)
                return BadRequest(uploadResult.Error.Message);

            var productImage = new ProductImage
            {
                ProductId = productId,
                ImageUrl = uploadResult.SecureUrl.ToString(),
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.ProductImage.AddAsync(productImage);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetProductImagesByProductId), new { productId }, productImage);
        }

        // GET: api/product-images/{productId} (Lấy danh sách ảnh theo ProductId)
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductImagesByProductId(int productId)
        {
            var images = await _unitOfWork.ProductImage.GetAllAsync(pi => pi.ProductId == productId);
            var imageDtos = _mapper.Map<IEnumerable<ProductImageReadDTO>>(images);
            return Ok(ResultModel.Success(imageDtos));
        }

        // GET: api/product-images (Lấy toàn bộ danh sách ảnh)
        [HttpGet]
        public async Task<IActionResult> GetAllProductImages()
        {
            var productImages = await _unitOfWork.ProductImage.GetAllAsync();
            var productImageDtos = _mapper.Map<IEnumerable<ProductImageReadDTO>>(productImages);
            return Ok(ResultModel.Success(productImageDtos));
        }

        // PUT: api/product-images/{id} (Cập nhật ảnh mới)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductImage(int id, IFormFile file)
        {
            var productImage = await _unitOfWork.ProductImage.GetByIdAsync(id);
            if (productImage == null)
            {
                return NotFound(new { message = $"Không tìm thấy ProductImage với Id = {id}" });
            }

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "product_images"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.Error != null)
                return BadRequest(uploadResult.Error.Message);

            productImage.ImageUrl = uploadResult.SecureUrl.ToString();
            productImage.CreatedDate = DateTime.UtcNow;

            _unitOfWork.ProductImage.Update(productImage);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Cập nhật thành công", productImage });
        }

        // DELETE: api/product-images/{id} (Xóa ảnh)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductImage(int id)
        {
            var productImage = await _unitOfWork.ProductImage.GetByIdAsync(id);
            if (productImage == null)
            {
                return NotFound(new { message = $"Không tìm thấy ProductImage với Id = {id}" });
            }

            _unitOfWork.ProductImage.Delete(productImage);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Xóa thành công", productImage });
        }
    }


}
