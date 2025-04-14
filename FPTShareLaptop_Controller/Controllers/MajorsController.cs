using AutoMapper;
using BusinessObjects.Models;
using DataAccess.MajorDTO;
using DataAccess.ResultModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/majors")]
    [ApiController]
    public class MajorsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MajorsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/majors
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var majors = await _unitOfWork.Major.GetAllAsync();
            var majorDTOs = _mapper.Map<IEnumerable<MajorReadDTO>>(majors);
            return Ok(ResultModel.Success(majorDTOs));
        }

        // GET: api/majors/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var major = await _unitOfWork.Major.GetByIdAsync(id);
            if (major == null)
                return NotFound(ResultModel.NotFound("Major not found."));

            var majorDTO = _mapper.Map<MajorReadDTO>(major);
            return Ok(ResultModel.Success(majorDTO));
        }

        // POST: api/majors
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MajorCreateDTO majorDTO)
        {
            if (majorDTO == null)
                return BadRequest(ResultModel.BadRequest("Invalid data."));

            var major = _mapper.Map<Major>(majorDTO);

            await _unitOfWork.Major.AddAsync(major);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<MajorReadDTO>(major);
            return CreatedAtAction(nameof(GetById), new { id = major.Id }, ResultModel.Created(result, "Major created successfully."));
        }

        // PUT: api/majors/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MajorUpdateDTO majorDTO)
        {
            if (majorDTO == null)
                return BadRequest(ResultModel.BadRequest("Invalid data."));

            var existingMajor = await _unitOfWork.Major.GetByIdAsync(id);
            if (existingMajor == null)
                return NotFound(ResultModel.NotFound("Major not found."));

            // Cập nhật duy nhất trường Status
            existingMajor.Status = majorDTO.Status;

            _unitOfWork.Major.Update(existingMajor);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Status updated successfully."));
        }

        // DELETE: api/majors/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var major = await _unitOfWork.Major.GetByIdAsync(id);
            if (major == null)
                return NotFound(ResultModel.NotFound("Major not found."));

            _unitOfWork.Major.Delete(major);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Deleted successfully."));
        }
    }
}
