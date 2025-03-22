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
    public class ShopsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ShopsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/shops
        [HttpGet]
        public async Task<IActionResult> GetShops()
        {
            var shops = await _unitOfWork.Shop.GetAllAsync();
            var shopDTOs = _mapper.Map<IEnumerable<ShopDTO>>(shops);
            return Ok(ResultModel.Success(shopDTOs));
        }

        // GET: api/shops/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShop(int id)
        {
            var shop = await _unitOfWork.Shop.GetByIdAsync(id);
            if (shop == null)
            {
                return NotFound(ResultModel.NotFound("Shop not found"));
            }

            var shopDTO = _mapper.Map<ShopDTO>(shop);
            return Ok(ResultModel.Success(shopDTO));
        }

        // POST: api/shops
        [HttpPost]
        public async Task<IActionResult> CreateShop([FromBody] ShopCreateDTO shopDTO)
        {
            if (shopDTO == null)
            {
                return BadRequest(ResultModel.BadRequest("Invalid data"));
            }

            var shop = _mapper.Map<Shop>(shopDTO);
            shop.CreatedDate = DateTime.UtcNow;
            shop.UpdatedDate = DateTime.UtcNow;

            await _unitOfWork.Shop.AddAsync(shop);
            await _unitOfWork.SaveAsync();

            var createdShopDTO = _mapper.Map<ShopDTO>(shop);
            return CreatedAtAction(nameof(GetShop), new { id = shop.ShopId }, ResultModel.Created(createdShopDTO));
        }

        // PUT: api/shops/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShop(int id, [FromBody] ShopUpdateDTO shopDTO)
        {
            if (shopDTO == null)
            {
                return BadRequest(ResultModel.BadRequest("Invalid data"));
            }

            var existingShop = await _unitOfWork.Shop.GetByIdAsync(id);
            if (existingShop == null)
            {
                return NotFound(ResultModel.NotFound("Shop not found"));
            }

            _mapper.Map(shopDTO, existingShop);
            existingShop.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Shop.Update(existingShop);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(_mapper.Map<ShopDTO>(existingShop), "Shop updated successfully"));
        }

        // DELETE: api/shops/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShop(int id)
        {
            var shop = await _unitOfWork.Shop.GetByIdAsync(id);
            if (shop == null)
            {
                return NotFound(ResultModel.NotFound("Shop not found"));
            }

            _unitOfWork.Shop.Delete(shop);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Shop deleted successfully"));
        }
    }
}
