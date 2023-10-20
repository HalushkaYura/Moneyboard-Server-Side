using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moneyboard.Core.DTO.ProjectDTO;
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
    public class ProjectService : IProjectService
    {
        protected readonly IMapper _mapper;
        protected readonly UserManager<User> _userManager;
        protected IRepository<Project> _projectRepository;
        protected IRepository<BankCard> _bankCardBaseRepository;
        //protected IBankCardRepository _bankCardRepository;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IRoleService _roleService;
        protected readonly IRepository<UserProject> _userProjectRepository;
        protected readonly IRepository<User> _userRepository;
        protected readonly IRepository<Role> _roleRepository;
        //protected readonly IProjectRepository _projectBaseRepository;
        //protected readonly IUserProjectRepository _userProjectBaseRepository;
        public ProjectService(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IRepository<Project> projectRepository,
            IRepository<BankCard> bankCardBaseRepository,
            IRoleService roleService,
            IRepository<User> userRepository,
            IRepository<Role> roleRepository,
            IRepository<UserProject> userProjectRepository,
            UserManager<User> userManager
                                                    //IProjectRepository projectBaseRepository,
                                                    //IUserProjectRepository userProjectBaseRepository
                                                    //IBankCardRepository bankCardRepository
                                                    )
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _projectRepository = projectRepository;
            _bankCardBaseRepository = bankCardBaseRepository;
            _roleService = roleService;
            _userProjectRepository = userProjectRepository;
            _userRepository = userRepository;
            _userManager = userManager;
            _roleRepository = roleRepository;
            //_projectBaseRepository = projectBaseRepository;
            //_userProjectRepository = userProjectBaseRepository;
            //_bankCardRepository = bankCardRepository;
        }


        public async Task<ProjectIdDTO> CreateNewProjectAsync(ProjectCreateDTO projectDTO, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.UserNotFound);

            var existingProjects = await _projectRepository.GetListAsync(
                p => p.Name == projectDTO.Name
                     && p.UserProjects.Any(up => up.UserId == userId));

            if (existingProjects.Any())
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, "Project with the same name already exists for this user.");

            var bankcard = _mapper.Map<BankCard>(projectDTO);
            var project = _mapper.Map<Project>(projectDTO);
            project.ProjectPoinPercent = 0;
            project.SalaryDate = GetSalaryDate(projectDTO.SalaryDay);
            bankcard.Money = project.BaseSalary * 2;

            if (await _bankCardBaseRepository.GetByCardNumberAsync(projectDTO.CardNumber) == null)
            {
                project.BankCard = bankcard;
                await _bankCardBaseRepository.AddAsync(bankcard);
                await _bankCardBaseRepository.SaveChangesAsync();
            }
            else
            {
                project.BankCard = await _bankCardBaseRepository.GetByCardNumberAsync(projectDTO.CardNumber);
                await _bankCardBaseRepository.SaveChangesAsync();
            }

            await _projectRepository.AddAsync(project);
            await _projectRepository.SaveChangesAsync();
            await CreateUserProjectAndRole(userId, project.ProjectId, "Owner", true);

            return new ProjectIdDTO { ProjectId = project.ProjectId };
        }

        private DateTime GetSalaryDate(int salaryDay)
        {
            DateTime today = DateTime.Today;

            if (today.Day >= salaryDay)
            {
                today = today.AddMonths(1);
            }

            DateTime targetDate = new DateTime(today.Year, today.Month, salaryDay);

            return targetDate;
        }
        public async Task AddMemberToProjectAsync(string userId, int projectId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.UserNotFound);

            await CreateUserProjectAndRole(userId, projectId, "Member", false);

        }

        private async Task CreateUserProjectAndRole(string userId, int projectId, string name, bool isOwner)
        {
            var project = await _projectRepository.GetByKeyAsync(projectId);

            if (project == null)
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.ProjectNotFound);

            var existingMemberRole = await _roleRepository.GetListAsync(
            r => r.RoleName == name && r.ProjectId == projectId);

            Role role;

            if (existingMemberRole.Any())
            {
                role = existingMemberRole.First();
            }
            else
            {
                role = new Role
                {
                    RoleName = name,
                    RolePoints = 0,
                    CreateDate = DateTime.Now,
                    ProjectId = projectId
                };

                await _roleRepository.AddAsync(role);
                await _roleRepository.SaveChangesAsync();
            }

            var userProject = new UserProject
            {
                IsOwner = isOwner,
                MemberDate = DateTime.Now,
                PersonalPoints = 0,
                UserId = userId,
                Project = project,
                Role = role
            };

            await _userProjectRepository.AddAsync(userProject);
            await _userProjectRepository.SaveChangesAsync();
        }

        public async Task<ProjectInfoDTO> InfoFromProjectAsync(int projectId, string userId)
        {
            var project = await _projectRepository.GetByKeyAsync(projectId);

            if (project == null)
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.ProjectNotFound);

            var userProject = await _userProjectRepository.GetUserProjectAsync(userId, projectId);

            if (userProject == null)
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.ProjectNotFound);

            bool isOwner = userProject != null && userProject.IsOwner == true;

            var projectInfo = _mapper.Map<ProjectInfoDTO>(project);
            projectInfo.IsOwner = isOwner;

            return projectInfo;
        }

        public async Task EditProjectDateAsync(ProjectEditDTO projectEditDTO, int projectId, string userId)
        {
            var project = await _projectRepository.GetByKeyAsync(projectId);

            if (project == null)
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.ProjectNotFound);


            project = _mapper.Map(projectEditDTO, project);


            await _projectRepository.UpdateAsync(project);
            await _projectRepository.SaveChangesAsync();

        }
        public async Task EditProjectPointPrecent(ProjectPointProcentDTO projectPointProcent, int projectId, string userId)
        {
            var project = await _projectRepository.GetByKeyAsync(projectId);
            if (project == null)
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.ProjectNotFound);

            project.ProjectPoinPercent = projectPointProcent.ProjectPoinPercent;

            await _projectRepository.UpdateAsync(project);
            await _projectRepository.SaveChangesAsync();
        }
        public async Task<IEnumerable<ProjectForUserDTO>> GetProjectsOwnedByUserAsync(string userId)
        {
            var userProjects = await _userProjectRepository.GetListAsync(up => up.UserId == userId && up.IsOwner);

            var projectIds = userProjects.Select(up => up.ProjectId);

            var projects = await _projectRepository.GetListAsync(p => projectIds.Contains(p.ProjectId));

            var projectDtos = _mapper.Map<IEnumerable<ProjectForUserDTO>>(projects);

            return projectDtos;
        }

        public async Task<IEnumerable<ProjectForUserDTO>> GetProjectsUserIsMemberAsync(string userId)
        {
            var userProjects = await _userProjectRepository.GetListAsync(up => up.UserId == userId && !up.IsOwner);

            var projectIds = userProjects.Select(up => up.ProjectId);

            var projects = await _projectRepository.GetListAsync(p => projectIds.Contains(p.ProjectId));

            var projectDtos = _mapper.Map<IEnumerable<ProjectForUserDTO>>(projects);

            return projectDtos;
        }
        public async Task<ProjectDetailsDTO> GetProjectDetailsAsync(int projectId, string userId)
        {
            var userProjectTest = await _userProjectRepository.GetListAsync(x => x.ProjectId == projectId && x.UserId == userId && (x.IsOwner != true && x.IsOwner != false));
            if (userProjectTest == null)
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.ProjectNotFound);

            var project = await _projectRepository.GetByKeyAsync(projectId);
            if (project == null)
            {
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.ProjectNotFound);
            }

            var projectDTO = _mapper.Map<ProjectDetailsDTO>(project);

            var userProjects = await _userProjectRepository.GetListAsync(x => x.ProjectId == projectId);
            var userProjectUserIds = userProjects.Select(up => up.UserId).ToList();
            var roles = await _roleRepository.GetListAsync(x => x.ProjectId == projectId);
            var users = await _userRepository.GetListAsync(x => userProjectUserIds.Contains(x.Id));


            var memberDTOs = _mapper.Map<List<ProjectMemberDTO>>(userProjects);

            var usersDTO = _mapper.Map<List<ProjectMemberDTO>>(users);
            var rolesDTO = _mapper.Map<List<ProjectMemberDTO>>(roles);

            for (var i = 0; i < memberDTOs.Count; i++)
            {
                // Знаходимо відповідність між об'єктами за Id
                var userDTO = usersDTO.FirstOrDefault(u => u.UserId == userProjects.ElementAt(i).UserId);
                var roleDTO = rolesDTO.FirstOrDefault(r => r.RoleId == userProjects.ElementAt(i).RoleId);

                // Просто присвоюємо значення з мапованих об'єктів
                memberDTOs[i].UserName = userDTO.UserName;
                memberDTOs[i].ImageUrl = userDTO.ImageUrl;
                memberDTOs[i].RoleName = roleDTO.RoleName;
                memberDTOs[i].RolePoints = roleDTO.RolePoints;
            }


            projectDTO.Members = memberDTOs;

            return projectDTO;
        }
    }
}
