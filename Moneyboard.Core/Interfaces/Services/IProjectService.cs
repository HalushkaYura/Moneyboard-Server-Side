using Moneyboard.Core.DTO.ProjectDTO;
using Moneyboard.Core.DTO.RoleDTO;
using Moneyboard.Core.DTO.UserDTO;
using Moneyboard.Core.Entities.ProjectEntity;
using Moneyboard.Core.Helpers;

namespace Moneyboard.Core.Interfaces.Services
{
    public interface IProjectService
    {
        Task<ProjectIdDTO> CreateNewProjectAsync(ProjectCreateDTO projectDTO, string userId);
        Task<ProjectInfoDTO> InfoFromProjectAsync(int projectId, string userId);
        Task EditProjectDateAsync(ProjectEditDTO projectEditDTO, int projectId, string userId);
        Task AddMemberToProjectAsync(string userId, int projectId);
        Task<IEnumerable<ProjectForUserDTO>> GetProjectsOwnedByUserAsync(string userId);
        Task<IEnumerable<ProjectForUserDTO>> GetProjectsUserIsMemberAsync(string userId);
        Task EditProjectPointPrecent(ProjectRolesDTO projectPointProcent, int projectId, string userId);
        Task<ProjectDetailsDTO> GetProjectDetailsAsync(int projectId, string userId);
        //Task<(IEnumerable<ProjectForUserDTO> Owners, IEnumerable<ProjectForUserDTO> Members)> GetAllProjectsForUserAsync(string userId);

        //Task CreateNewRoleAsync(int projectId, ProjectCreate2DTO roleCreateDTO);

    }
}
