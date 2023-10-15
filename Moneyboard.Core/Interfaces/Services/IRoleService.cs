using Moneyboard.Core.DTO.RoleDTO;

namespace Moneyboard.Core.Interfaces.Services
{
    public interface IRoleService
    {
        Task CreateNewRoleAsync(int projectId, RoleCreateDTO roleCreateDTO);
        Task EditRoleDateAsync(int roleId, RoleEditDTO roleEditDTO);
        Task AssignRoleToProjectMemberAsync(string userId, int projectId, string roleName);

    }
}
