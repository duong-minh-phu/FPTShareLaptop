using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.BorrowContractDTO;
using DataAccess.BorrowRequestDTO;
using Microsoft.AspNetCore.Http;

namespace Service.IService
{
    public interface IBorrowContractService
    {
        Task<List<BorrowContractResponseModel>> GetAllBorrowContracts();
        Task<BorrowContractResponseModel> GetBorrowContractById(int contractId);
        Task<BorrowContractResponseModel> CreateBorrowContract(CreateBorrowContractReqModel requestModel);
        Task UpdateBorrowContract(string token, int contractId, UpdateBorrowContractReqModel requestModel);
        Task DeleteBorrowContract(string token, int contractId);
        Task UploadSignedContractImage(string token, int contractId, UploadBorrowContractReqModel requestModel);
    }
}
