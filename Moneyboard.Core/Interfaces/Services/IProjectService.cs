using Moneyboard.Core.DTO.ProjectDTO;
using Moneyboard.Core.DTO.RoleDTO;
using Moneyboard.Core.Entities.ProjectEntity;
using Moneyboard.Core.Helpers;

namespace Moneyboard.Core.Interfaces.Services
{
    public interface IProjectService
    {
        Task CreateProjectAsync(ProjectCreateDTO projectDTO);
       // Task CreateRoleInActiveProjectAsync(RoleCreateDTO roleCreateDTO, int projectContext);
    }
}
