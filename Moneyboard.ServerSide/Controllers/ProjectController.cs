using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moneyboard.Core.DTO.BankCardDTO;
using Moneyboard.Core.DTO.ProjectDTO;
using Moneyboard.Core.Interfaces.Services;
using Moneyboard.Core.Services;
using System.Security.Claims;

namespace Moneyboard.ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly Core.Interfaces.Services.IProjectService _projectService;
        private readonly IRoleService _roleService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IBankCardService _bankCardService;
        private string UserId => User.FindFirst(ClaimTypes.NameIdentifier).Value;

        public ProjectController(
            Core.Interfaces.Services.IProjectService projectService,
            IRoleService roleService,
            IBankCardService bankCardService,
            IBackgroundJobClient backgroundJobClient)
        {
            _projectService = projectService;
            _roleService = roleService;
            _bankCardService = bankCardService;
            _backgroundJobClient = backgroundJobClient;
            _backgroundJobClient = backgroundJobClient;
        }

        [Authorize]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateProjectAsync([FromBody] ProjectCreateDTO projectCreateDTO)
        {
            var id = await _projectService.CreateNewProjectAsync(projectCreateDTO, UserId);

            return Ok(id);
        }

        [Authorize]
        [HttpPost]
        [Route("add-member/{projectId}")]
        public async Task<IActionResult> AddMemberToProjectAsync(int projectId)
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
        public async Task<IActionResult> UpdatePointProcent([FromBody] ProjectPointDTO projectPointProcentDTO, int projectId)
        {
            await _projectService.EditProjectPointPrecent(projectPointProcentDTO, projectId);
            return Ok();
        }
        [Authorize]
        [HttpPut]
        [Route("personal-point/{projectId}/{userId}")]
        public async Task<IActionResult> UpdatePersonalPoint([FromBody] PersonalPointDTO personalPointDTO, int projectId, string userId)
        {
            await _projectService.EditPersonalPoint(personalPointDTO, projectId, userId);
            return Ok();
        }
        [Authorize]
        [HttpGet]
        [Route("details/{projectId}")]
        public async Task<IActionResult> ProjectTableInfo(int projectId)
        {
            var info = await _projectService.GetProjectDetailsAsync(projectId, UserId);
            return Ok(info);
        }


        [Authorize]
        [HttpDelete]
        [Route("delete/{projectId}")]
        public async Task<IActionResult> DeleteRole(int projectId)
        {
            await _projectService.DeleteProjectAsync(projectId, UserId);
            return Ok("Project deleted successfully");
        }

        [Authorize]
        [HttpDelete]
        [Route("leave/{projectId}")]
        public async Task<IActionResult> Leave(int projectId)
        {
            await _projectService.LeaveTheProjectAsync(projectId, UserId);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("calculate-total-payments/{projectId}")]
        public async Task<IActionResult> CalculateTotalPayments(int projectId)
        {
            var totalPayments = await _projectService.CalculateTotalPayments(projectId);
            return Ok(totalPayments);
        }

            [Authorize]
            [HttpPost]
            [Route("process-salary/{projectId}")]
            public IActionResult ScheduleProcessSalary(int projectId)
            {
            RecurringJob.AddOrUpdate($"ProccesSalary_Project_{projectId}", () => _projectService.ProccesSalary(projectId), Cron.Daily(10));
            return Ok("Processing salary scheduled.");
            }


    }
}
