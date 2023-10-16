using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moneyboard.Core.DTO.RoleDTO;
using Moneyboard.Core.DTO.UserDTO;
using Moneyboard.Core.Entities.BankCardEntity;
using Moneyboard.Core.Entities.ProjectEntity;
using Moneyboard.Core.Entities.RoleEntity;
using Moneyboard.Core.Entities.UserEntity;
using Moneyboard.Core.Entities.UserProjectEntity;
using Moneyboard.Core.Exeptions;
using Moneyboard.Core.Interfaces.Repository;
using Moneyboard.Core.Interfaces.Services;
using Moneyboard.Core.Resources;
using System.Data;

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
        public async Task CreateNewRoleAsync(int projectId, RoleCreateDTO roleCreateDTO)
        {
            var project = await _projectRepository.GetByKeyAsync(projectId);
            if (project == null)
            {
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.ProjectNotFound);
            }
            var roles = await _roleRepository.GetAllAsync();
            bool roleExists = roles.Any(r => r.RoleName == roleCreateDTO.RoleName && r.ProjectId == projectId);
            if (roleExists)
            {
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.FileNameAlreadyExist);
            }

            var role = _mapper.Map<Role>(roleCreateDTO);

            role.Project = project;
            await _roleRepository.AddAsync(role);

            await _projectRepository.SaveChangesAsync();
            await _roleRepository.SaveChangesAsync();
        }

        public async Task EditRoleDateAsync(int roleId, RoleEditDTO roleEditDTO)
        {
            var role = await _roleRepository.GetByKeyAsync(roleId);

            if (role == null)
            {
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, "Role not found.");
            }

            role.RoleName = roleEditDTO.Name;
            role.RolePoints = roleEditDTO.RolePoints;
            role.CreateDate = DateTime.Now;


            await _roleRepository.UpdateAsync(role);
        }

        public async Task AssignRoleToProjectMemberAsync(string userId, int projectId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.UserNotFound);
            }

            var userProject = await _userProjectRepository.GetByPairOfKeysAsync(userId, projectId);

            if (userProject == null)
            {
                // Обробка помилки, якщо користувач не є членом проекту
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.UserNotMember);
            }

            await _userManager.AddToRoleAsync(user, roleName);
        }

    }
}
