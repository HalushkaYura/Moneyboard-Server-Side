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
        public async Task<IActionResult> AssignRoleToProjectMember([FromBody] RoleAssignmentRoleDTO roleAssignmentRoleDTO)
        {

            await _roleService.AssignRoleToProjectMemberAsync(roleAssignmentRoleDTO);
            return Ok();
        }

        [Authorize]
        [HttpPut]
        [Route("edit/{projectId}")]
        public async Task<IActionResult> EditRole([FromBody] RoleEditDTO roleEditDTO, int projectId)
        {
            await _roleService.EditRoleDateAsync(roleEditDTO, projectId);
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
        [Route("delete/{roleId}")]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            await _roleService.DeleteRoleAsync(roleId, UserId);
            return Ok("Role deleted successfully");
        }
    }
}
