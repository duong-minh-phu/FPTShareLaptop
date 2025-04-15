using AutoMapper;
using BusinessObjects.Models;
using DataAccess.ResultModel;
using DataAccess.SponsorContributionDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/sponsor-contributions")]
    [ApiController]
    public class SponsorContributionsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SponsorContributionsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contributions = await _unitOfWork.SponsorContribution.GetAllAsync();
            var contributionDTOs = _mapper.Map<IEnumerable<SponsorContributionReadDTO>>(contributions);
            return Ok(ResultModel.Success(contributionDTOs));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contribution = await _unitOfWork.SponsorContribution.GetByIdAsync(id);
            if (contribution == null)
                return NotFound(ResultModel.NotFound());

            var contributionDTO = _mapper.Map<SponsorContributionReadDTO>(contribution);
            return Ok(ResultModel.Success(contributionDTO));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SponsorContributionCreateDTO dto)
        {
            if (dto == null)
                return BadRequest(ResultModel.BadRequest("Invalid data."));

            var contribution = _mapper.Map<SponsorContribution>(dto);

            await _unitOfWork.SponsorContribution.AddAsync(contribution);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<SponsorContributionReadDTO>(contribution);
            return CreatedAtAction(nameof(GetById), new { id = contribution.SponsorContributionId }, ResultModel.Created(result));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SponsorContributionUpdateDTO dto)
        {
            if (dto == null)
                return BadRequest(ResultModel.BadRequest("Invalid data."));

            var contribution = await _unitOfWork.SponsorContribution.GetByIdAsync(id);
            if (contribution == null)
                return NotFound(ResultModel.NotFound());

            _mapper.Map(dto, contribution);
            _unitOfWork.SponsorContribution.Update(contribution);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var contribution = await _unitOfWork.SponsorContribution.GetByIdAsync(id);
            if (contribution == null)
                return NotFound(ResultModel.NotFound());

            _unitOfWork.SponsorContribution.Delete(contribution);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Deleted successfully"));
        }
    }
}
