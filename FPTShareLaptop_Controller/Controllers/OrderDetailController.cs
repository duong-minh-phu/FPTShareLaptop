using AutoMapper;
using BusinessObjects.Models;
using DataAccess.OrderDetailDTO;
using DataAccess.ProductDTO;
using DataAccess.ResultModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.IService;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/orderdetails")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderDetailController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/orderdetails (Lấy danh sách chi tiết đơn hàng)
        [HttpGet]
        public async Task<IActionResult> GetOrderDetails()
        {
            var orderDetails = await _unitOfWork.OrderDetail.GetAllAsync();
            var orderDetailDtos = _mapper.Map<IEnumerable<OrderDetailReadDTO>>(orderDetails);
            return Ok(ResultModel.Success(orderDetailDtos));
        }

        // GET: api/orderdetails/{id} (Lấy chi tiết đơn hàng theo ID)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetailById(int id)
        {
            var orderDetail = await _unitOfWork.OrderDetail.GetByIdAsync(id);
            if (orderDetail == null)
                return NotFound(ResultModel.NotFound($"Không tìm thấy chi tiết đơn hàng với ID = {id}"));

            var orderDetailDto = _mapper.Map<OrderDetailReadDTO>(orderDetail);
            return Ok(ResultModel.Success(orderDetailDto));
        }

        // POST: api/orderdetails (Tạo chi tiết đơn hàng mới)
        [HttpPost]
        public async Task<IActionResult> CreateMultipleOrderDetails([FromBody] List<OrderDetailCreateDTO> orderDetailDtos)
        {
            if (orderDetailDtos == null || !orderDetailDtos.Any())
            {
                return BadRequest(ResultModel.BadRequest("Danh sách chi tiết đơn hàng không hợp lệ."));
            }

            var createdDetails = new List<OrderDetail>();

            foreach (var dto in orderDetailDtos)
            {
                var orderDetail = _mapper.Map<OrderDetail>(dto);
                await _unitOfWork.OrderDetail.AddAsync(orderDetail);
                createdDetails.Add(orderDetail);
            }

            await _unitOfWork.SaveAsync();

            var resultDtos = _mapper.Map<List<OrderDetailReadDTO>>(createdDetails);
            return Ok(ResultModel.Created(resultDtos, "Danh sách chi tiết đơn hàng đã được tạo."));
        }

        // PUT: api/orderdetails/{id} (Cập nhật chi tiết đơn hàng)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderDetail(int id, [FromBody] OrderDetailUpdateDTO orderDetailDto)
        {
            var orderDetail = await _unitOfWork.OrderDetail.GetByIdAsync(id);
            if (orderDetail == null)
                return NotFound(ResultModel.NotFound($"Không tìm thấy chi tiết đơn hàng với ID = {id}"));

            _mapper.Map(orderDetailDto, orderDetail);

            _unitOfWork.OrderDetail.Update(orderDetail);
            await _unitOfWork.SaveAsync();

            var orderDetailReadDto = _mapper.Map<OrderDetailReadDTO>(orderDetail);
            return Ok(ResultModel.Success(orderDetailReadDto, "Chi tiết đơn hàng đã được cập nhật."));
        }

        // DELETE: api/orderdetails/{id} (Xóa chi tiết đơn hàng)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            var orderDetail = await _unitOfWork.OrderDetail.GetByIdAsync(id);
            if (orderDetail == null)
                return NotFound(ResultModel.NotFound($"Không tìm thấy chi tiết đơn hàng với ID = {id}"));

            _unitOfWork.OrderDetail.Delete(orderDetail);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success($"Chi tiết đơn hàng với ID {id} đã được xóa."));
        }


        [HttpGet("product-sales")]
        public async Task<IActionResult> GetProductSalesStatistics()
        {
            // Lấy tất cả chi tiết đơn hàng
            var orderDetails = await _unitOfWork.OrderDetail.GetAllAsync();

            // Kiểm tra nếu không có dữ liệu
            if (orderDetails == null || !orderDetails.Any())
            {
                return NotFound(ResultModel.NotFound("Không có thông tin bán hàng cho sản phẩm."));
            }

            // Nhóm theo ProductId và tính tổng số lượng bán được
            var productSalesStatistics = orderDetails
                .GroupBy(od => od.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalQuantitySold = g.Sum(od => od.Quantity) // Sử dụng trực tiếp Quantity vì đã chắc chắn là int
                })
                .ToList();

            return Ok(ResultModel.Success(productSalesStatistics));
        }


    }
}
