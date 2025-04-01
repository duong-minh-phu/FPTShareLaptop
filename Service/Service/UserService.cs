using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.UserDTO;
using Service.IService;
using Service.Utils.CustomException;

namespace Service.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;          
            _mapper = mapper;
        }
        public async Task<List<UserProfileModel>> GetAllUsers()
        {
            var users = await _unitOfWork.Users.GetAllAsync(includeProperties: u => u.Role);
            return _mapper.Map<List<UserProfileModel>>(users);
        }

        public async Task<UserProfileModel> GetUserById(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId, includeProperties: u => u.Role);
            if (user == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "User not found");
            }
            return _mapper.Map<UserProfileModel>(user);
        }
    }
}
