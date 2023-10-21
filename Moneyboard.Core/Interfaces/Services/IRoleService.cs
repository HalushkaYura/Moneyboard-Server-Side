using Moneyboard.Core.DTO.RoleDTO;

namespace Moneyboard.Core.Interfaces.Services
{
    public interface IRoleService
    {
        Task CreateNewRoleAsync(int projectId);
        Task EditRoleDateAsync(RoleEditDTO roleEditDTO);
        Task AssignRoleToProjectMemberAsync(string userId, int projectId, int roleId);
        Task<List<RoleInfoDTO>> GetRolesByProjectIdAsync(int projectId);
        Task DeleteRoleAsync(int roleId, string userId);
    }
}
