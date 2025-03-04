using DataAccess.RoleDTO;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using System.Net;
using System.Threading.Tasks;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return StatusCode((int)HttpStatusCode.OK, new { IsSuccess = true, Code = (int)HttpStatusCode.OK, Message = "Roles retrieved successfully.", Data = roles });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
                return StatusCode((int)HttpStatusCode.NotFound, new { IsSuccess = false, Code = (int)HttpStatusCode.NotFound, Message = "Role not found." });

            return StatusCode((int)HttpStatusCode.OK, new { IsSuccess = true, Code = (int)HttpStatusCode.OK, Message = "Role retrieved successfully.", Data = role });
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleModel model)
        {
            await _roleService.CreateRoleAsync(model);
            return StatusCode((int)HttpStatusCode.Created, new { IsSuccess = true, Code = (int)HttpStatusCode.Created, Message = "Role created successfully." });
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleDTO updatedRole)
        {
            var success = await _roleService.UpdateRoleAsync(id, updatedRole.RoleName);
            if (!success)
                return StatusCode((int)HttpStatusCode.NotFound, new { IsSuccess = false, Code = (int)HttpStatusCode.NotFound, Message = "Role not found." });

            return StatusCode((int)HttpStatusCode.OK, new { IsSuccess = true, Code = (int)HttpStatusCode.OK, Message = "Role updated successfully." });
        }
    }
}
