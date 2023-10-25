using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moneyboard.Core.DTO.RoleDTO;
using Moneyboard.Core.Entities.ProjectEntity;
using Moneyboard.Core.Entities.RoleEntity;
using Moneyboard.Core.Entities.UserEntity;
using Moneyboard.Core.Entities.UserProjectEntity;
using Moneyboard.Core.Exeptions;
using Moneyboard.Core.Interfaces.Repository;
using Moneyboard.Core.Interfaces.Services;
using Moneyboard.Core.Resources;

namespace Moneyboard.Core.Services
{
    public class RoleService : IRoleService
    {
        protected readonly IMapper _mapper;
        protected IRepository<Project> _projectRepository;
        protected IRepository<Role> _roleRepository;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly UserManager<User> _userManager;
        protected readonly IRepository<UserProject> _userProjectRepository;

        public RoleService(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IRepository<Project> projectRepository,
            IRepository<Role> roleRepository,
            UserManager<User> userManager,
            IRepository<UserProject> userProjectRepository)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _projectRepository = projectRepository;
            _roleRepository = roleRepository;
            _userManager = userManager;
            _userProjectRepository = userProjectRepository;
        }
        public async Task CreateNewRoleAsync(int projectId)
        {
            var project = await _projectRepository.GetByKeyAsync(projectId);
            if (project == null)
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.ProjectNotFound);

            var role = new Role
            {
                IsDefolt = null,
                Project = project,
                RoleName = "New role",
                RolePoints = 0,

            };


            await _roleRepository.AddAsync(role);
            await _projectRepository.SaveChangesAsync();
            await _roleRepository.SaveChangesAsync();
        }

        public async Task EditRoleDateAsync(RoleEditDTO roleEditDTO, int projectId)
        {
            var roles = await _roleRepository.GetAllAsync();
            bool roleExists = roles.Any(r => r.RoleName == roleEditDTO.RoleName && r.ProjectId == projectId);
            if (roles.Count(r => r.RoleName == roleEditDTO.RoleName && r.ProjectId == projectId) >= 2)
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, "Duplicate roles found");

            var role = await _roleRepository.GetByKeyAsync(roleEditDTO.RoleId);
            if (role == null)
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, "Role not found");

            role.RoleName = roleEditDTO.RoleName;
            role.RolePoints = roleEditDTO.RolePoints;
            role.CreateDate = DateTime.Now.Date;

            await _roleRepository.UpdateAsync(role);
            await _roleRepository.SaveChangesAsync();
        }

        public async Task AssignRoleToProjectMemberAsync(RoleAssignmentRoleDTO roleAssignmentRoleDTO, int projectId)
        {
            var user = await _userManager.FindByIdAsync(roleAssignmentRoleDTO.UserId);
            if (user == null)
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.UserNotFound);

            var userProject = await _userProjectRepository.GetEntityAsync(x => x.UserId == roleAssignmentRoleDTO.UserId && x.ProjectId == projectId);
            if (userProject == null)
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.UserNotMember);

            var role = await _roleRepository.GetByKeyAsync(roleAssignmentRoleDTO.RoleId);
            if (role == null)
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, "Role not foud");

            userProject.RoleId = roleAssignmentRoleDTO.RoleId;
            await _userProjectRepository.UpdateAsync(userProject);
            await _userProjectRepository.SaveChangesAsync();
        }
        public async Task<List<RoleInfoDTO>> GetRolesByProjectIdAsync(int projectId)
        {
            var project = await _projectRepository.GetByKeyAsync(projectId);
            if (project == null)
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.ProjectNotFound);

            var roles = await _roleRepository.GetListAsync(r => r.ProjectId == projectId);
            var roleDTO = _mapper.Map<List<RoleInfoDTO>>(roles);
            return roleDTO;
        }

        public async Task DeleteRoleAsync(int roleId, string userId)
        {
            var role = await _roleRepository.GetByKeyAsync(roleId);
            if (role == null || role.IsDefolt != null)
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, "The action cannot be performed");

            var userProject = await _userProjectRepository.GetEntityAsync(x => x.UserId == userId && x.ProjectId == role.ProjectId);
            if (userProject == null )
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, "Not enough rights");
            
            await AutoAssignDefaultRoleAsync(roleId, role.ProjectId);
            await _roleRepository.DeleteAsync(role);
            await _roleRepository.SaveChangesAsync();
        }
         
        private async Task AutoAssignDefaultRoleAsync(int roleId, int projectId)
        {
            var usersWithDeletedRole = await _userProjectRepository.GetListAsync(x => x.RoleId == roleId && x.ProjectId == projectId);

            var defaultRole = await _roleRepository.GetEntityAsync(x => x.ProjectId == projectId && x.IsDefolt == false);

            foreach (var userProject in usersWithDeletedRole)
            {
                userProject.Role = defaultRole;
                userProject.RoleId = defaultRole.RoleId;
                await _userProjectRepository.UpdateAsync(userProject);
            }

            await _userProjectRepository.SaveChangesAsync();
        }
    }
}
