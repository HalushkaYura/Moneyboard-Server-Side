using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moneyboard.Core.DTO.BankCardDTO;
using Moneyboard.Core.DTO.ProjectDTO;
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
        private readonly IBankCardService _bankCardService;
        private string UserId => User.FindFirst(ClaimTypes.NameIdentifier).Value;

        public ProjectController(
            Core.Interfaces.Services.IProjectService projectService,
            IRoleService roleService,
            IBankCardService bankCardService)
        {
            _projectService = projectService;
            _roleService = roleService;
            _bankCardService = bankCardService;
        }

        [Authorize]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateProjectAsync([FromBody] ProjectCreateDTO projectCreateDTO)
        {
            var id =  await _projectService.CreateNewProjectAsync(projectCreateDTO, UserId);

            return Ok(id);
        }

        [Authorize]
        [HttpPost]
        [Route("add-member/{projectId}")]
        public async Task<IActionResult> AddMemberToProjectAsync( int projectId)
        {
            await _projectService.AddMemberToProjectAsync(UserId, projectId);
            return Ok();
        }



        [Authorize]
        [HttpGet]
        [Route("info/{projectId}")]
        public async Task<IActionResult> ProjectInfoAsync(int projectId)
        {
            var projectInfo = await _projectService.InfoFromProjectAsync(projectId, UserId);

            return Ok(projectInfo);
        }

        [Authorize]
        [HttpGet]
        [Route("owner")]
        public async Task<IActionResult> GetProjectsOwnedByUserAsync()
        {
            var project = await _projectService.GetProjectsOwnedByUserAsync(UserId);

            return Ok(project);
        }

        [Authorize]
        [HttpGet]
        [Route("member")]
        public async Task<IActionResult> GetProjectsUserIsMemberAsync()
        {
            var project = await _projectService.GetProjectsUserIsMemberAsync(UserId);

            return Ok(project);
        }

        [Authorize]
        [HttpPut]
        [Route("editProject/{projectId}")]
        public async Task<IActionResult> EditProject([FromBody] ProjectEditDTO projectEditDTO, int projectId)
        {

            await _projectService.EditProjectDateAsync(projectEditDTO, projectId, UserId);
            return Ok("Проект успішно оновлено");
        }

        [Authorize]
        [HttpPut]
        [Route("editBankCard/{projectId}")]
        public async Task<IActionResult> EditBankCard([FromBody] BankCardEditDTO bankCardEditDTO, int projectId)
        {

            await _bankCardService.EditBankCardDateAsync(bankCardEditDTO, projectId, UserId);
            return Ok("Проект успішно оновлено");
        }

        [Authorize]
        [HttpGet]
        [Route("infoBankCard/{projectId}")]
        public async Task<IActionResult> GetBankCard(int projectId)
        {
            var info = await _bankCardService.InfoBankCardAsync(projectId, UserId);
            return Ok(info);
        }

        [Authorize]
        [HttpPut]
        [Route("point/{projectId}")]
        public async Task<IActionResult> UpdatePointProcent(ProjectPointProcentDTO projectPointProcentDTO, int projectId)
        {
            await _projectService.EditProjectPointPrecent(projectPointProcentDTO, projectId, UserId);
            return Ok();
        }
        [Authorize]
        [HttpGet]
        [Route("details/{projectId}")]
        public async Task<IActionResult> ProjectTableInfo( int projectId)
        {
            var info  = await _projectService.GetProjectDetailsAsync(projectId, UserId);
            return Ok(info);
        }

    }
}
