using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.BorrowRequestDTO;

namespace Service.IService
{
    public interface IBorrowRequestService
    {
        Task<List<BorrowRequestResModel>> GetAllBorrowRequests();
        Task<BorrowRequestResModel> GetBorrowRequestById(int requestId);
        Task<BorrowRequestResModel> CreateBorrowRequest(string token ,CreateBorrowRequestReqModel requestModel);
        Task UpdateBorrowRequest(int requestId, UpdateBorrowRequestReqModel updateModel);
        Task DeleteBorrowRequest(int requestId);
    }
}
