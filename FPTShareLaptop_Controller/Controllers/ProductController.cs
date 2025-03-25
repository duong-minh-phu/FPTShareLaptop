using AutoMapper;
using BusinessObjects.Models;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using DataAccess.ProductDTO;
using DataAccess.ResultModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper, Cloudinary cloudinary)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinary = cloudinary;
        }

        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _unitOfWork.Product.GetAllAsync(
                includeProperties: p => p.Category);
            var productDTOs = _mapper.Map<IEnumerable<ProductReadDTO>>(products);
            return Ok(ResultModel.Success(productDTOs));
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _unitOfWork.Product.GetByIdAsync(id, p => p.Category);
            if (product == null)
            {
                return NotFound(ResultModel.NotFound());
            }
            var productDTO = _mapper.Map<ProductReadDTO>(product);
            return Ok(ResultModel.Success(productDTO));
        }

        // POST: api/products (Thêm mới sản phẩm + upload ảnh)
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateDTO productDTO)
        {
            if (productDTO == null)
            {
                return BadRequest(ResultModel.BadRequest("Invalid data."));
            }

            // Upload ảnh lên Cloudinary nếu có
            string imageUrl = null;
            if (productDTO.ImageFile != null && productDTO.ImageFile.Length > 0)
            {
                using var stream = productDTO.ImageFile.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(productDTO.ImageFile.FileName, stream),
                    Folder = "product_images"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.Error != null)
                    return BadRequest(ResultModel.BadRequest(uploadResult.Error.Message));

                imageUrl = uploadResult.SecureUrl.ToString();
            }

            var product = _mapper.Map<Product>(productDTO);
            product.ImageProduct = imageUrl;
            product.CreatedDate = DateTime.UtcNow;
            product.UpdatedDate = DateTime.UtcNow;

            await _unitOfWork.Product.AddAsync(product);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<ProductReadDTO>(product);
            return CreatedAtAction(nameof(GetById), new { id = product.ProductId }, ResultModel.Created(result));
        }

        // PUT: api/products/{id} (Cập nhật sản phẩm + upload ảnh mới nếu có)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] ProductUpdateDTO productDTO)
        {
            if (productDTO == null || productDTO.ProductId != id)
            {
                return BadRequest(ResultModel.BadRequest("ID mismatch."));
            }

            var existingProduct = await _unitOfWork.Product.GetByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound(ResultModel.NotFound());
            }

            // Upload ảnh mới nếu có
            if (productDTO.ImageFile != null && productDTO.ImageFile.Length > 0)
            {
                using var stream = productDTO.ImageFile.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(productDTO.ImageFile.FileName, stream),
                    Folder = "product_images"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.Error != null)
                    return BadRequest(ResultModel.BadRequest(uploadResult.Error.Message));

                existingProduct.ImageProduct = uploadResult.SecureUrl.ToString();
            }

            _mapper.Map(productDTO, existingProduct);
            existingProduct.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Product.Update(existingProduct);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Updated successfully"));
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.Product.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound(ResultModel.NotFound());
            }

            _unitOfWork.Product.Delete(product);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Deleted successfully"));
        }
    }

}
