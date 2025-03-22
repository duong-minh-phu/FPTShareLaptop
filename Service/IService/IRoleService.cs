using DataAccess.RoleDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IRoleService
    {
        Task<List<RoleDTO>> GetAllRolesAsync();
        Task<RoleDTO> GetRoleByIdAsync(int id);
        Task CreateRoleAsync(CreateRoleModel model);
        Task<bool> UpdateRoleAsync(int roleId, string roleName);
    }
}
