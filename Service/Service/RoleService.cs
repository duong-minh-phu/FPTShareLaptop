using BusinessObjects.Models;
using DataAccess.RoleDTO;
using Service.IService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Service
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<RoleDTO>> GetAllRolesAsync()
        {
            var roles = await _unitOfWork.Role.GetAllAsync();
            return roles.Select(r => new RoleDTO { RoleName = r.RoleName }).ToList(); 
        }


        public async Task<RoleDTO> GetRoleByIdAsync(int id)
        {
            var role = await _unitOfWork.Role.GetByIdAsync(id);
            if (role == null) return null;
            return new RoleDTO { RoleName = role.RoleName };
        }

        public async Task CreateRoleAsync(CreateRoleModel model)
        {
            var newRole = new Role { RoleId = model.RoleId, RoleName = model.RoleName };
            await _unitOfWork.Role.AddAsync(newRole);
            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> UpdateRoleAsync(int roleId, string roleName)
        {
            var role = await _unitOfWork.Role.GetByIdAsync(roleId);
            if (role == null) return false;

            role.RoleName = roleName;
            _unitOfWork.Role.Update(role);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
