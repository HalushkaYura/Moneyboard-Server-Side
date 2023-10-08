using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Moneyboard.ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly Core.Interfaces.Services.IProjectService _projectService;
        public RoleController(Core.Interfaces.Services.IProjectService projectService)
        {
            _projectService = projectService;

        }
    }
}
