using Moneyboard.Core.DTO.ProjectDTO;
using Moneyboard.Core.Entities.ProjectEntity;

namespace Moneyboard.Core.Interfaces.Services
{
    public interface IProjectService
    {
        Task CreateProjectAsync(ProjectCreateDTO projectDTO);

    }
}
