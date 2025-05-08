using AutoMapper;
using BusinessObjects.Models;
using DataAccess.OrderDTO;
using DataAccess.ResultModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/orders (Lấy danh sách đơn hàng)
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _unitOfWork.Order.GetAllAsync();
            var orderDtos = _mapper.Map<IEnumerable<OrderReadDTO>>(orders);
            return Ok(ResultModel.Success(orderDtos));
        }

        // GET: api/orders/{id} (Lấy đơn hàng theo ID)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _unitOfWork.Order.GetByIdAsync(id);
            if (order == null)
                return NotFound(ResultModel.NotFound($"Không tìm thấy đơn hàng với ID = {id}"));

            var orderDto = _mapper.Map<OrderReadDTO>(order);
            return Ok(ResultModel.Success(orderDto));
        }

        // POST: api/orders (Tạo đơn hàng mới)
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDTO orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            order.CreatedDate = DateTime.UtcNow;

            await _unitOfWork.Order.AddAsync(order);
            await _unitOfWork.SaveAsync();

            var orderReadDto = _mapper.Map<OrderReadDTO>(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, ResultModel.Success(orderReadDto, "Đơn hàng đã được tạo."));
        }

        // PUT: api/orders/{id} (Cập nhật đơn hàng)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderUpdateDTO orderDto)
        {
            var order = await _unitOfWork.Order.GetByIdAsync(id);
            if (order == null)
                return NotFound(ResultModel.NotFound($"Không tìm thấy đơn hàng với ID = {id}"));

            _mapper.Map(orderDto, order);
            order.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Order.Update(order);
            await _unitOfWork.SaveAsync();

            var orderReadDto = _mapper.Map<OrderReadDTO>(order);
            return Ok(ResultModel.Success(orderReadDto, "Đơn hàng đã được cập nhật."));
        }

        // DELETE: api/orders/{id} (Xóa đơn hàng)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _unitOfWork.Order.GetByIdAsync(id);
            if (order == null)
                return NotFound(ResultModel.NotFound($"Không tìm thấy đơn hàng với ID = {id}"));

            _unitOfWork.Order.Delete(order);
            await _unitOfWork.SaveAsync();

            return Ok(ResultModel.Success($"Đơn hàng với ID {id} đã được xóa."));
        }
    }
}
