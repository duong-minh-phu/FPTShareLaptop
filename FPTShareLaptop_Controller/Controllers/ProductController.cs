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
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Cloudinary _cloudinary;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper, Cloudinary cloudinary)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinary = cloudinary;
        }

        // GET: api/products (Lấy danh sách sản phẩm)
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _unitOfWork.Product.GetAllAsync();
            var productDtos = _mapper.Map<IEnumerable<ProductReadDTO>>(products);
            return Ok(ResultModel.Success(productDtos));
        }

        // GET: api/products/{id} (Lấy sản phẩm theo ID)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _unitOfWork.Product.GetByIdAsync(id);
            if (product == null)
                return NotFound(ResultModel.NotFound($"Không tìm thấy sản phẩm với ID = {id}"));

            var productDto = _mapper.Map<ProductReadDTO>(product);
            return Ok(ResultModel.Success(productDto));
        }

        // POST: api/products (Tạo sản phẩm mới)
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] IFormFile file, [FromForm] ProductCreateDTO productDto)
        {
            if (file == null || file.Length == 0)
                return BadRequest(ResultModel.BadRequest("File ảnh không hợp lệ."));

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "product_images"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.Error != null)
                return BadRequest(ResultModel.BadRequest(uploadResult.Error.Message));

            var product = _mapper.Map<Product>(productDto);
            product.ImageProduct = uploadResult.SecureUrl.ToString();
            product.CreatedDate = DateTime.UtcNow;

            await _unitOfWork.Product.AddAsync(product);
            await _unitOfWork.SaveAsync();

            var productReadDto = _mapper.Map<ProductReadDTO>(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, ResultModel.Success(productReadDto, "Sản phẩm đã được tạo."));
        }

        // PUT: api/products/{id} (Cập nhật sản phẩm)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] IFormFile? file, [FromForm] ProductUpdateDTO productDto)
        {
            var product = await _unitOfWork.Product.GetByIdAsync(id);
            if (product == null)
                return NotFound(ResultModel.NotFound($"Không tìm thấy sản phẩm với ID = {id}"));

            _mapper.Map(productDto, product);
            product.UpdatedDate = DateTime.UtcNow;

            // Nếu có file mới thì upload lên Cloudinary
            if (file != null)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "product_images"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.Error != null)
                    return BadRequest(ResultModel.BadRequest(uploadResult.Error.Message));

                product.ImageProduct = uploadResult.SecureUrl.ToString();
            }

            _unitOfWork.Product.Update(product);
            await _unitOfWork.SaveAsync();

            var productReadDto = _mapper.Map<ProductReadDTO>(product);
            return Ok(ResultModel.Success(productReadDto, "Sản phẩm đã được cập nhật."));
        }

        // DELETE: api/products/{id} (Xóa sản phẩm)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _unitOfWork.Product.GetByIdAsync(id);
            if (product == null)
                return NotFound(ResultModel.NotFound($"Không tìm thấy sản phẩm với ID = {id}"));

            _unitOfWork.Product.Delete(product);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success($"Sản phẩm với ID {id} đã được xóa."));
        }
    }
}
