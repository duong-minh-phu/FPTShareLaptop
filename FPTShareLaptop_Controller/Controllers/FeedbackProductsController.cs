using AutoMapper;
using BusinessObjects.Models;
using DataAccess.FeedbackProductDTO;
using DataAccess.ResultModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/feedback-products")]
    [ApiController]
    public class FeedbackProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FeedbackProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/feedback-products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var feedbacks = await _unitOfWork.FeedbackProduct.GetAllAsync();
            var feedbackDTOs = _mapper.Map<IEnumerable<FeedbackProductDTO>>(feedbacks);
            return Ok(ResultModel.Success(feedbackDTOs));
        }

        // GET: api/feedback-products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var feedback = await _unitOfWork.FeedbackProduct.GetByIdAsync(id);
            if (feedback == null)
                return NotFound(ResultModel.NotFound());

            var feedbackDTO = _mapper.Map<FeedbackProductDTO>(feedback);
            return Ok(ResultModel.Success(feedbackDTO));
        }

        // GET: api/feedback-products/by-product/{productId}
        [HttpGet("by-product/{productId}")]
        public async Task<IActionResult> GetByProductId(int productId)
        {
            
            var feedbacks = await _unitOfWork.FeedbackProduct.GetAllAsync(ii => ii.ProductId == productId);
            var feedbackDTOs = _mapper.Map<IEnumerable<FeedbackProductDTO>>(feedbacks);
            return Ok(ResultModel.Success(feedbackDTOs));
        }

        // POST: api/feedback-products
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FeedbackProductCreateDTO feedbackDTO)
        {
            if (feedbackDTO == null)
                return BadRequest(ResultModel.BadRequest("Invalid data."));

            var feedback = _mapper.Map<FeedbackProduct>(feedbackDTO);
            feedback.FeedbackDate = DateTime.UtcNow;

            await _unitOfWork.FeedbackProduct.AddAsync(feedback);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<FeedbackProductDTO>(feedback);
            return CreatedAtAction(nameof(GetById), new { id = feedback.FeedbackProductId }, ResultModel.Created(result));
        }

        // PUT: api/feedback-products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] FeedbackProductUpdateDTO feedbackDTO)
        {
            if (feedbackDTO == null || feedbackDTO.Id != id)
                return BadRequest(ResultModel.BadRequest("ID mismatch."));

            var existingFeedback = await _unitOfWork.FeedbackProduct.GetByIdAsync(id);
            if (existingFeedback == null)
                return NotFound(ResultModel.NotFound());

            _mapper.Map(feedbackDTO, existingFeedback);
            

            _unitOfWork.FeedbackProduct.Update(existingFeedback);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Updated successfully"));
        }

        // DELETE: api/feedback-products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var feedback = await _unitOfWork.FeedbackProduct.GetByIdAsync(id);
            if (feedback == null)
                return NotFound(ResultModel.NotFound());

            _unitOfWork.FeedbackProduct.Delete(feedback);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Deleted successfully"));
        }
    }
}
