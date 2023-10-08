using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moneyboard.Core.DTO.RoleDTO;
using Moneyboard.Core.Entities.BankCardEntity;
using Moneyboard.Core.Entities.ProjectEntity;
using Moneyboard.Core.Entities.RoleEntity;
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
        public async Task CreateNewRoleAsync(RoleCreateDTO roleCreateDTO, int projectId)
        {
            var roles= await _roleRepository.GetAllAsync();
            bool roleExists = roles.Any(r => r.RoleName == roleCreateDTO.Name && r.ProjectId == projectId);
            if (!roleExists)
            {
                var project = await _projectRepository.GetByKeyAsync(projectId);
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
    }
}
