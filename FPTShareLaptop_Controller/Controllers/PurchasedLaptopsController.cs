using AutoMapper;
using BusinessObjects.Models;
using DataAccess.PurchasedLaptopDTO;
using DataAccess.ResultModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasedLaptopsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PurchasedLaptopsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var laptops = await _unitOfWork.PurchasedLaptop.GetAllAsync();
            var result = _mapper.Map<IEnumerable<PurchasedLaptopDTO>>(laptops);
            return Ok(ResultModel.Success(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var laptop = await _unitOfWork.PurchasedLaptop.GetByIdAsync(id);
            if (laptop == null)
                return NotFound(ResultModel.NotFound());

            var result = _mapper.Map<PurchasedLaptopDTO>(laptop);
            return Ok(ResultModel.Success(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PurchasedLaptopCreateDTO dto)
        {
            if (dto == null)
                return BadRequest(ResultModel.BadRequest("Invalid data."));

            var newLaptop = _mapper.Map<PurchasedLaptop>(dto);
            await _unitOfWork.PurchasedLaptop.AddAsync(newLaptop);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<PurchasedLaptopDTO>(newLaptop);
            return Ok(ResultModel.Created(result, "Created successfully"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PurchasedLaptopUpdateDTO dto)
        {
            var existing = await _unitOfWork.PurchasedLaptop.GetByIdAsync(id);
            if (existing == null)
                return NotFound(ResultModel.NotFound());

            _mapper.Map(dto, existing);
            _unitOfWork.PurchasedLaptop.Update(existing);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var laptop = await _unitOfWork.PurchasedLaptop.GetByIdAsync(id);
            if (laptop == null)
                return NotFound(ResultModel.NotFound());

            _unitOfWork.PurchasedLaptop.Delete(laptop);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Deleted successfully"));
        }
    }
}
