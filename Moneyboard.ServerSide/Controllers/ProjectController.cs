using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moneyboard.Core.DTO.ProjectDTO;
using Moneyboard.Core.DTO.RoleDTO;
using Moneyboard.Core.DTO.UserDTO;
using Moneyboard.Core.Entities.UserEntity;
using Moneyboard.Core.Interfaces.Services;
using Moneyboard.Core.Services;

namespace Moneyboard.ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly Core.Interfaces.Services.IProjectService _projectService;
        private readonly IProjectContext _projectContext;
        private readonly IRoleService _roleService;
        public ProjectController(
            Core.Interfaces.Services.IProjectService projectService,
            IProjectContext projectContext,
            IRoleService roleService)
        {
            _projectService = projectService;
            _projectContext = projectContext;
            _roleService = roleService;
        }

        [Authorize]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateProjectAsync([FromBody] ProjectCreateDTO projectCreateDTO)
        {
            await _projectService.CreateProjectAsync(projectCreateDTO);

            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("createRole")]
        public async Task<IActionResult> CreateRoleAsync([FromBody] RoleCreateDTO roleCreateDTO)
        {
            int activeProjectId = _projectContext.ActiveProjectId;
            await _roleService.CreateNewRoleAsync(roleCreateDTO, activeProjectId);

            return Ok();
        }



    }
}
