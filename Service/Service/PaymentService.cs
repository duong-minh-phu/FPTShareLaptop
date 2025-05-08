using System.Linq.Expressions;
using System.Net;
using System.Security.Cryptography.Xml;
using System.Transactions;
using AutoMapper;
using BusinessObjects.Models;
using DataAccess.PaymentDTO;
using DataAccess.PayOSDTO;
using DataAccess.WalletDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Net.payOS.Types;
using Newtonsoft.Json;
using Service.IService;
using Service.Utils.CustomException;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Service.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJWTService _jwtService;
        private readonly IPayOSService _payOSService;
        private readonly IWalletService _walletService;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper, IJWTService jWTService, IPayOSService pOSService, IWalletService walletService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtService = jWTService;
            _payOSService = pOSService;
            _walletService = walletService;
        }

        public async Task HandlePaymentWebhookAsync(WebhookType webhookBody)
        {
            // 1. Xác thực webhook
            var webhookData = await _payOSService.VerifyPaymentWebhookData(webhookBody);

            // 2. Lấy TransactionCode từ webhookData (chính là orderCode)
            var transactionCode = webhookData.orderCode.ToString();

            // 3. Tìm Payment theo TransactionCode
            var payment = await _unitOfWork.Payment.FirstOrDefaultAsync(p => p.TransactionCode == transactionCode);
            if (payment == null) throw new ApiException(HttpStatusCode.NotFound, "Payment not found.");

            // 4. Tìm Order theo Payment
            var order = await _unitOfWork.Order.GetByIdAsync(payment.OrderId);
            if (order == null) throw new ApiException(HttpStatusCode.NotFound, "Order not found.");

            // 5. Cập nhật trạng thái
            if (webhookBody.success)
            {          
                payment.Status = "Paid";
                order.Status = "Success";
          
            }

            await _walletService.DisburseToManagerAsync(payment.Amount);

            var log = new TransactionLog
            {
                UserId = order.UserId,
                TransactionType = "Payment",
                Amount = payment.Amount,
                CreatedDate = DateTime.UtcNow,
                Note = $"Payment for Order #{order.OrderId} - Successful, amount: {payment.Amount}",
                ReferenceId = payment.PaymentId,
                SourceTable = "Payment"
            };
            await _unitOfWork.TransactionLog.AddAsync(log);
                                 
            var orderDetails = await _unitOfWork.OrderDetail.GetAllAsync(od => od.OrderId == order.OrderId);
            if (orderDetails == null || !orderDetails.Any())
                throw new ApiException(HttpStatusCode.NotFound, "Order details not found.");

            // 8. Tính toán số tiền cần chuyển cho mỗi shop
            var transfers = new List<ShopTransferReqModel>();
            decimal feeRate = 0.05m; // Giả sử phí 5%, có thể lấy từ webhook hoặc cấu hình.

            foreach (var orderDetail in orderDetails)
            {
                var product = await _unitOfWork.Product.GetByIdAsync(orderDetail.ProductId);
                if (product == null)
                    throw new ApiException(HttpStatusCode.NotFound, $"Product with ID {orderDetail.ProductId} not found.");

                // Tính số tiền gốc (chưa trừ phí)
                var existingTransfer = transfers.FirstOrDefault(t => t.ShopId == product.ShopId);
                var rawAmount = orderDetail.PriceItem * orderDetail.Quantity;

                if (existingTransfer == null)
                {
                    transfers.Add(new ShopTransferReqModel
                    {
                        ShopId = product.ShopId,
                        Amount = rawAmount, // chưa trừ phí
                        OrderId = order.OrderId,                
                    });
                }
                else
                {
                    existingTransfer.Amount += rawAmount; // cộng dồn gốc
                }
            }

            // Trừ phí 1 lần duy nhất sau khi đã cộng tổng
            foreach (var transfer in transfers)
            {
                var total = transfer.Amount;
                var fee = Math.Round(total * feeRate, 2);
                transfer.Fee = fee;
                transfer.Amount = Math.Round(total - fee, 0); // Trừ phí tại đây
            }

            // Gọi hàm tự động transfer cho từng Shop
            await _walletService.TransferFromManagerToShopsAsync(transfers, feeRate);            
            _unitOfWork.Payment.Update(payment);
            _unitOfWork.Order.Update(order);

            await _unitOfWork.SaveAsync();

        }



        public async Task<PaymentViewResModel> CreatePaymentAsync(string token, int orderId, int paymentMethodId)
        {
            var userId = _jwtService.decodeToken(token, "userId");
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "User not found.");

            var order = await _unitOfWork.Order.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, $"Order with ID {orderId} not found.");
            }

            var paymentMethod = await _unitOfWork.PaymentMethod.GetByIdAsync(paymentMethodId);
            if (paymentMethod == null)
            {
                throw new ApiException(HttpStatusCode.NotFound,$"Payment method with ID {paymentMethodId} is invalid.");
            }

            Payment newPayment = new Payment
            {
                OrderId = order.OrderId,
                PaymentMethodId = paymentMethod.PaymentMethodId,
                Amount = order.TotalPrice,  
                PaymentDate = DateTime.UtcNow, 
                Status = "Unpaid",
                TransactionCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(),
            };

            await _unitOfWork.Payment.AddAsync(newPayment);
            await _unitOfWork.SaveAsync();
       

            await _unitOfWork.SaveAsync();

            return new PaymentViewResModel
            {
                PaymentId = newPayment.PaymentId,
                OrderId = newPayment.OrderId,
                PaymentMethodId = newPayment.PaymentMethodId,
                Email = user.Email,   
                FullName = user.FullName,
                Amount = newPayment.Amount,
                Status = newPayment.Status,
                PaymentDate = newPayment.PaymentDate,
                TransactionCode = newPayment.TransactionCode
            };
        }

        public async Task<List<PaymentViewResModel>> GetAllPayment()
        {
            var payments = await _unitOfWork.Payment.GetAllAsync(includeProperties: new Expression<Func<Payment, object>>[] { p => p.Order, p => p.Order.User });
            return _mapper.Map<List<PaymentViewResModel>>(payments);
        }

        public async Task<PaymentViewResModel> GetPaymentByIdAsync(int paymentId)
        {
            var payment = await _unitOfWork.Payment.GetByIdAsync(paymentId ,includeProperties: new Expression<Func<Payment, object>>[] { p => p.Order, p => p.Order.User});
            return _mapper.Map<PaymentViewResModel>(payment);
        }

        public async Task<string> GetPaymentUrlAsync(HttpContext context, int paymentId, string redirectUrl)
        {
            var currPayment = await _unitOfWork.Payment.GetByIdAsync(paymentId);
            if (currPayment == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Payment does not exist");
            }

            if (currPayment.Status == "Paid")
            {
                throw new ApiException(HttpStatusCode.BadRequest, "The payment has already been paid");
            }

            PayOSReqModel payOSReqModel = new PayOSReqModel
            {
                OrderId = long.Parse(currPayment.TransactionCode),
                ProductName = "Thanh toán đơn hàng",
                Amount = currPayment.Amount/1000,
                RedirectUrl = redirectUrl,
                CancelUrl = redirectUrl
            };

            var result = await _payOSService.CreatePaymentUrl(payOSReqModel);

            return result.checkoutUrl;
        }

        public async Task UpdatePayment(string transactionCode, UpdatePaymentReqModel model)
        {
            var payment = await _unitOfWork.Payment.FirstOrDefaultAsync(p => p.TransactionCode == transactionCode);
            if (payment == null) throw new ApiException(HttpStatusCode.NotFound, "Payment not found.");

            
            var order = await _unitOfWork.Order.GetByIdAsync(payment.OrderId);
            if (order == null) throw new ApiException(HttpStatusCode.NotFound, "Order not found.");

            var status = model.Status;

            if (status == "CANCELLED")
            {                
                payment.Status = "Cancelled";
                order.Status = "Cancelled";
             
            }
            else if (status == "FAILED")
            {
                payment.Status = "Failed";
                order.Status = "Failed";              
            }

            _unitOfWork.Payment.Update(payment);
            _unitOfWork.Order.Update(order);
            await _unitOfWork.SaveAsync();
        }
    }
}
