using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moneyboard.Core.DTO.RoleDTO;
using Moneyboard.Core.DTO.UserDTO;
using Moneyboard.Core.Entities.BankCardEntity;
using Moneyboard.Core.Entities.ProjectEntity;
using Moneyboard.Core.Entities.RoleEntity;
using Moneyboard.Core.Entities.UserEntity;
using Moneyboard.Core.Exeptions;
using Moneyboard.Core.Interfaces.Repository;
using Moneyboard.Core.Interfaces.Services;
using ServiceStack;
using System.Data;

namespace Moneyboard.Core.Services
{
    public class RoleService : IRoleService
    {
        protected readonly IMapper _mapper;
        protected IRepository<Project> _projectRepository;
        protected IRepository<Role> _roleRepository;
        protected readonly IHttpContextAccessor _httpContextAccessor;


        public RoleService(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IRepository<Project> projectRepository,
            IRepository<Role> roleRepository)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _projectRepository = projectRepository;
            _roleRepository = roleRepository;

        }
        public async Task CreateNewRoleAsync(int projectId,RoleCreateDTO roleCreateDTO)
        {
            var project = await _projectRepository.GetByKeyAsync(projectId);
            if (project == null)
            {
                throw new Exception("Проект з вказаним projectId не знайдено.");
            }
            var roles= await _roleRepository.GetAllAsync();
            bool roleExists = roles.Any(r => r.RoleName == roleCreateDTO.Name && r.ProjectId == projectId);
            if (!roleExists)
            {
                var role = _mapper.Map<Role>(roleCreateDTO);

                role.Project = project;
                await _roleRepository.AddAsync(role);
            }
            else
            {
                throw new Exception("Така роль вже існує.");
            }

            await _projectRepository.SaveChangesAsync();
            await _roleRepository.SaveChangesAsync();
        }

        public async Task EditRoleDateAsync(int roleId, RoleEditDTO roleEditDTO)
        {
            var role = await _roleRepository.GetByKeyAsync(roleId);

            if (role == null)
            {
                // Якщо користувача не знайдено, можливо, ви можете виконати обробку помилки
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.FileNotExistsFmt);
            }

            role.RoleName = roleEditDTO.Name;
            role.RolePoints = roleEditDTO.RolePoints;
            role.CreateDate = DateTime.Now;


            await _roleRepository.UpdateAsync(role);
        }
    }
}
