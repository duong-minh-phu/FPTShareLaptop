using AutoMapper;
using BusinessObjects.Models;
using DataAccess.DonateItemDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonateItemController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DonateItemController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/DonateItem
        [HttpGet]
        public async Task<IActionResult> GetDonateItems()
        {
            var donateItems = await _unitOfWork.DonateItem.GetAllAsync();
            var donateItemsDTO = _mapper.Map<IEnumerable<DonateItemDTO>>(donateItems);
            return Ok(donateItemsDTO);
        }

        // GET: api/DonateItem/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDonateItem(int id)
        {
            var donateItem = await _unitOfWork.DonateItem.GetByIdAsync(id);
            if (donateItem == null)
            {
                return NotFound();
            }
            var donateItemDTO = _mapper.Map<DonateItemDTO>(donateItem);
            return Ok(donateItemDTO);
        }

        // POST: api/DonateItem
        [HttpPost]
        public async Task<IActionResult> CreateDonateItem([FromBody] DonateItemDTO donateItemDTO)
        {
            if (donateItemDTO == null)
            {
                return BadRequest("Invalid data.");
            }

            var donateItem = _mapper.Map<DonateItem>(donateItemDTO);
            await _unitOfWork.DonateItem.AddAsync(donateItem);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetDonateItem), new { id = donateItem.ItemId }, _mapper.Map<DonateItemDTO>(donateItem));
        }

        // PUT: api/DonateItem/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDonateItem(int id, [FromBody] DonateItemDTO donateItemDTO)
        {
            if (donateItemDTO == null || donateItemDTO.ItemId != id)
            {
                return BadRequest("Item ID mismatch.");
            }

            var existingItem = await _unitOfWork.DonateItem.GetByIdAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }

            _mapper.Map(donateItemDTO, existingItem);
            _unitOfWork.DonateItem.Update(existingItem);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        // DELETE: api/DonateItem/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonateItem(int id)
        {
            var donateItem = await _unitOfWork.DonateItem.GetByIdAsync(id);
            if (donateItem == null)
            {
                return NotFound();
            }

            _unitOfWork.DonateItem.Delete(donateItem);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
