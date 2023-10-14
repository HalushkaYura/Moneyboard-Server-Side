using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moneyboard.Core.DTO.ProjectDTO;
using Moneyboard.Core.DTO.RoleDTO;
using Moneyboard.Core.Interfaces.Services;
using System.Security.Claims;

namespace Moneyboard.ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly Core.Interfaces.Services.IProjectService _projectService;
        private readonly IRoleService _roleService;
        private string UserId => User.FindFirst(ClaimTypes.NameIdentifier).Value;

        public ProjectController(
            Core.Interfaces.Services.IProjectService projectService,
            IRoleService roleService)
        {
            _projectService = projectService;
            _roleService = roleService;
        }

        [Authorize]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateProjectAsync([FromBody] ProjectCreateDTO projectCreateDTO)
        {
            await _projectService.CreateNewProjectAsync(projectCreateDTO, UserId);

            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("createRole")]
        public async Task<IActionResult> CreateRoleAsync([FromBody] RoleCreateDTO roleCreateDTO)
        {
            await _roleService.CreateNewRoleAsync(roleCreateDTO.projectId, roleCreateDTO);

            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("info/{projectId}")]
        public async Task<IActionResult> ProjectInfoAsync(int projectId)
        {
            var projectInfo = await _projectService.InfoFromProjectAsync(projectId);

            return Ok(projectInfo);
        }

        [Authorize]
        [HttpGet]
        [Route("allProject")]
        public async Task<IActionResult> AllProjectOfUserAsync()
        {
            var projectId = await _projectService.AllUserProjectAsync(UserId);

            return Ok(projectId);
        }

        [Authorize]
        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> EditProject([FromBody] ProjectEditDTO projectEditDTO)
        {
            try
            {
                await _projectService.EditProjectDateAsync(projectEditDTO, projectEditDTO.projectId);
                return Ok("Проект успішно оновлено");
            }
            catch (Exception ex)
            {
                return BadRequest($"Помилка при оновленні проекту: {ex.Message}");
            }
        }

    }
}
