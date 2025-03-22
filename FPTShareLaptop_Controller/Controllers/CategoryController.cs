using AutoMapper;
using BusinessObjects.Models;
using DataAccess.CategoryDTO;
using DataAccess.ResultModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/categories
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            var categoryDTOs = _mapper.Map<IEnumerable<CategoryDTO>>(categories);

            return Ok(ResultModel.Success(categoryDTOs, "Categories retrieved successfully."));
        }

        // GET: api/categories/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound(ResultModel.NotFound("Category not found."));
            }

            var categoryDTO = _mapper.Map<CategoryDTO>(category);
            return Ok(ResultModel.Success(categoryDTO, "Category retrieved successfully."));
        }

        // POST: api/categories
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDTO categoryDTO)
        {
            if (categoryDTO == null)
            {
                return BadRequest(ResultModel.BadRequest("Invalid category data."));
            }

            var category = _mapper.Map<Category>(categoryDTO);
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveAsync();

            var createdCategoryDTO = _mapper.Map<CategoryDTO>(category);
            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, ResultModel.Created(createdCategoryDTO, "Category created successfully."));
        }

        // PUT: api/categories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDTO categoryDTO)
        {
            if (categoryDTO == null || categoryDTO.CategoryId != id)
            {
                return BadRequest(ResultModel.BadRequest("Category ID mismatch."));
            }

            var existingCategory = await _unitOfWork.Categories.GetByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound(ResultModel.NotFound("Category not found."));
            }

            _mapper.Map(categoryDTO, existingCategory);
            _unitOfWork.Categories.Update(existingCategory);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Category updated successfully."));
        }

        // DELETE: api/categories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound(ResultModel.NotFound("Category not found."));
            }

            _unitOfWork.Categories.Delete(category);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Category deleted successfully."));
        }
    }

}
