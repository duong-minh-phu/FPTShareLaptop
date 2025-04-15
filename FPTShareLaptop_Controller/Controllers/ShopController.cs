using AutoMapper;
using BusinessObjects.Models;
using DataAccess.ResultModel;
using DataAccess.ShopDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/shops")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ShopController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/shops
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var shops = await _unitOfWork.Shop.GetAllAsync();
            var result = _mapper.Map<IEnumerable<ShopReadDTO>>(shops);
            return Ok(ResultModel.Success(result, "Shops retrieved successfully"));
        }

        // GET: api/shops/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var shop = await _unitOfWork.Shop.GetByIdAsync(id);
            if (shop == null)
                return NotFound(ResultModel.NotFound("Shop not found"));

            var result = _mapper.Map<ShopReadDTO>(shop);
            return Ok(ResultModel.Success(result));
        }

        // POST: api/shops
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ShopCreateDTO createDto)
        {
            if (createDto == null)
                return BadRequest(ResultModel.BadRequest("Invalid shop data"));

            var shop = _mapper.Map<Shop>(createDto);
            await _unitOfWork.Shop.AddAsync(shop);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<ShopReadDTO>(shop);
            return CreatedAtAction(nameof(GetById), new { id = shop.ShopId }, ResultModel.Created(result, "Shop created successfully"));
        }

        // PUT: api/shops/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ShopUpdateDTO updateDto)
        {
            var existingShop = await _unitOfWork.Shop.GetByIdAsync(id);
            if (existingShop == null)
                return NotFound(ResultModel.NotFound("Shop not found"));

            _mapper.Map(updateDto, existingShop);
            _unitOfWork.Shop.Update(existingShop);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<ShopReadDTO>(existingShop);
            return Ok(ResultModel.Success(result, "Shop updated successfully"));
        }

        // DELETE: api/shops/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var shop = await _unitOfWork.Shop.GetByIdAsync(id);
            if (shop == null)
                return NotFound(ResultModel.NotFound("Shop not found"));

            _unitOfWork.Shop.Delete(shop);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Shop deleted successfully"));
        }
    }
}
