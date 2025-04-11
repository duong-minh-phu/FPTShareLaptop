using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.PaymentMethodDTO;

namespace Service.IService
{
    public interface IPaymentMethodService
    {
        Task<List<PaymentMethodResModel>> GetAllAsync();
        Task<PaymentMethodResModel> GetByIdAsync(int id);
        Task<PaymentMethodResModel> AddAsync(PaymentMethodReqModel model);
        Task UpdateAsync(string token, int id, PaymentMethodReqModel model);
        Task DeleteAsync(string token, int id);
    }
}
