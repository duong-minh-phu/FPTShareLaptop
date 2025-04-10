using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.BorrowContractDTO;
using DataAccess.UserDTO;

namespace Service.IService
{
    public interface IUserService
    {
        Task<List<UserProfileModel>> GetAllUsers();
        Task<UserProfileModel> GetUserById(int userId);
    }
}
