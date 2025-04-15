using AutoMapper;
using BusinessObjects.Models;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using DataAccess.ContractImageDTO;
using DataAccess.ResultModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using Microsoft.CodeAnalysis;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/contract-images")]
    [ApiController]
    public class ContractImagesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;

        public ContractImagesController(IUnitOfWork unitOfWork, IMapper mapper, Cloudinary cloudinary)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinary = cloudinary;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var images = await _unitOfWork.ContractImage.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<ContractImageDTO>>(images);
            return Ok(ResultModel.Success(dtos, "Fetched all contract images."));
        }

        [HttpGet("by-contract/{contractId}")]
        public async Task<IActionResult> GetByContractId(int contractId)
        {
            var images = await _unitOfWork.ContractImage.GetAllAsync(pi => pi.BorrowContractId == contractId);
            var dtos = _mapper.Map<IEnumerable<ContractImageDTO>>(images);
            return Ok(ResultModel.Success(dtos, "Fetched contract images by contract."));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var image = await _unitOfWork.ContractImage.GetByIdAsync(id);
            if (image == null)
                return NotFound(ResultModel.NotFound("Image not found."));

            return Ok(ResultModel.Success(_mapper.Map<ContractImageDTO>(image), "Fetched image successfully."));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(IFormFile file, [FromForm] ContractImageCreateDTO dto)
        {
            if (dto == null || file == null || file.Length == 0)
                return BadRequest(ResultModel.BadRequest("Invalid input."));

            string imageUrl;
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "contract_images"
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
                return BadRequest(ResultModel.BadRequest(uploadResult.Error.Message));

            imageUrl = uploadResult.SecureUrl.ToString();

            var contractImage = _mapper.Map<ContractImage>(dto);
            contractImage.ImageUrl = imageUrl;
            contractImage.CreatedDate = DateTime.UtcNow;

            await _unitOfWork.ContractImage.AddAsync(contractImage);
            await _unitOfWork.SaveAsync();

            return StatusCode(201, ResultModel.Created(_mapper.Map<ContractImageDTO>(contractImage), "Created successfully."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var img = await _unitOfWork.ContractImage.GetByIdAsync(id);
            if (img == null)
                return NotFound(ResultModel.NotFound("Image not found."));

            _unitOfWork.ContractImage.Delete(img);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success(null, "Deleted successfully."));
        }
    }
}
