using AutoMapper;
using BusinessObjects.Models;
using DataAccess.ProductImageDTO;
using DataAccess.ResultModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/product-images")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductImageController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/product-images
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productImages = await _unitOfWork.ProductImage.GetAllAsync();
            var result = _mapper.Map<IEnumerable<ProductImageReadDTO>>(productImages);
            return Ok(ResultModel.Success(result, "Product images retrieved successfully"));
        }

        // GET: api/product-images/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var productImage = await _unitOfWork.ProductImage.GetByIdAsync(id);
            if (productImage == null)
                return NotFound(ResultModel.NotFound("Product image not found"));

            var result = _mapper.Map<ProductImageReadDTO>(productImage);
            return Ok(ResultModel.Success(result));
        }

        // POST: api/product-images
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductImageCreateDTO createDto)
        {
            if (createDto == null)
                return BadRequest(ResultModel.BadRequest("Invalid product image data"));

            var productImage = _mapper.Map<ProductImage>(createDto);
            await _unitOfWork.ProductImage.AddAsync(productImage);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<ProductImageReadDTO>(productImage);
            return CreatedAtAction(nameof(GetById), new { id = productImage.ProductImageId }, ResultModel.Created(result, "Product image created successfully"));
        }

        // PUT: api/product-images/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductImageUpdateDTO updateDto)
        {
            var existingProductImage = await _unitOfWork.ProductImage.GetByIdAsync(id);
            if (existingProductImage == null)
                return NotFound(ResultModel.NotFound("Product image not found"));

            _mapper.Map(updateDto, existingProductImage);
            _unitOfWork.ProductImage.Update(existingProductImage);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<ProductImageReadDTO>(existingProductImage);
            return Ok(ResultModel.Success(result, "Product image updated successfully"));
        }

        // DELETE: api/product-images/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var productImage = await _unitOfWork.ProductImage.GetByIdAsync(id);
            if (productImage == null)
                return NotFound(ResultModel.NotFound("Product image not found"));

            _unitOfWork.ProductImage.Delete(productImage);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Product image deleted successfully"));
        }
    }
}
