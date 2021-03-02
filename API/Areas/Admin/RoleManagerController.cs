using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Services.Interface;

namespace API.Areas.Admin
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    [ApiController]
    [Route("api/[area]/[controller]/[action]")]
    public class RoleManagerController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleManagerController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateRole(CreateRoleDto createRoleDto)
        {
            var response = await _roleService.CreateRole(createRoleDto.RoleName);

            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpGet]
        public async Task<ActionResult> Roles()
        {
            var result = await _roleService.Roles();
            if (result.Success == false)
            {
                return BadRequest(result);
            }

            return Ok(result.Data);
        }


        [HttpPost]
        public async Task<ActionResult> RemoveUserFromRole(AddOrRemoveUserFromRoleDto removeUserFromRoleDto)
        {
            var response = await _roleService.RemoveUserFromRole(removeUserFromRoleDto);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }


        [HttpPost]
        public async Task<ActionResult> AddUserToRole(AddOrRemoveUserFromRoleDto addUserToRoleDto)
        {
            var response = await _roleService.AddUserToRole(addUserToRoleDto);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }
    }
}
