using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moneyboard.Core.DTO.RoleDTO;
using Moneyboard.Core.Interfaces.Services;
using System.Security.Claims;

namespace Moneyboard.ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly Core.Interfaces.Services.IProjectService _projectService;
        private readonly IRoleService _roleService;
        private string UserId => User.FindFirst(ClaimTypes.NameIdentifier).Value;

        public RoleController(
            Core.Interfaces.Services.IProjectService projectService,
            IRoleService roleService)
        {
            _projectService = projectService;
            _roleService = roleService;
        }

        [Authorize]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateRoleAsync([FromBody] RoleCreateDTO roleCreateDTO)
        {
            await _roleService.CreateNewRoleAsync(roleCreateDTO.projectId, roleCreateDTO);

            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("role-assignment")]
        public async Task<IActionResult> AssignRoleToProjectMember(string userId, int projectId, string roleName)
        {

            await _roleService.AssignRoleToProjectMemberAsync(userId, projectId, roleName);
            return Ok();
        }

        [Authorize]
        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> EditRole(int roleId, [FromBody] RoleEditDTO roleEditDTO)
        {
            await _roleService.EditRoleDateAsync(roleId, roleEditDTO);
            return Ok();
        }
    }
}
