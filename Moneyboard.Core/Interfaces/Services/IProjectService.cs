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
        Task EditProjectPointPrecent(ProjectPointDTO projectPointProcent, int projectId);
        Task<ProjectDetailsDTO> GetProjectDetailsAsync(int projectId, string userId);
        Task DeleteProjectAsync(int projectId, string userId);
        Task LeaveTheProjectAsync(int projectId, string userId);
        Task EditProjectPointAsync(int projectId, ProjectPointDTO projectRoles);
        Task<double> CalculateTotalPayments(int projectId);
        Task ProccesSalary(int projectId);



        //Task<(IEnumerable<ProjectForUserDTO> Owners, IEnumerable<ProjectForUserDTO> Members)> GetAllProjectsForUserAsync(string userId);

        //Task CreateNewRoleAsync(int projectId, ProjectCreate2DTO roleCreateDTO);

    }
}
