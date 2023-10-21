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
        [Route("create/{projectId}")]
        public async Task<IActionResult> CreateRoleAsync(int projectId)
        {
            await _roleService.CreateNewRoleAsync(projectId);
            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("role-assignment")]
        public async Task<IActionResult> AssignRoleToProjectMember(string userId, int projectId, int roleId)
        {

            await _roleService.AssignRoleToProjectMemberAsync(userId, projectId, roleId);
            return Ok();
        }

        [Authorize]
        [HttpPut]
        [Route("edit/{roleId}")]
        public async Task<IActionResult> EditRole(int roleId, [FromBody] RoleEditDTO roleEditDTO)
        {
            await _roleService.EditRoleDateAsync(roleId, roleEditDTO);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("project/{projectId}")]
        public async Task<IActionResult> GetRolesByProjectId(int projectId)
        {
            var roles = await _roleService.GetRolesByProjectIdAsync(projectId);
            return Ok(roles);
        }

        [Authorize]
        [HttpDelete]
        [Route("{roleId}")]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            await _roleService.DeleteRoleAsync(roleId, UserId);
            return Ok("Role deleted successfully");
        }
    }
}
