using Moneyboard.Core.DTO.RoleDTO;

namespace Moneyboard.Core.Interfaces.Services
{
    public interface IRoleService
    {
        Task CreateNewRoleAsync(int projectId);
        Task EditRoleDateAsync(RoleEditDTO roleEditDTO, int projectId);
        Task AssignRoleToProjectMemberAsync(RoleAssignmentRoleDTO roleAssignmentRoleDTO);
        Task<List<RoleInfoDTO>> GetRolesByProjectIdAsync(int projectId);
        Task DeleteRoleAsync(int roleId, string userId);
    }
}
