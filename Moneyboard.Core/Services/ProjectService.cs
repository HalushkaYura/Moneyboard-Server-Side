using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        protected readonly IProjectRepository _projectBaseRepository;
        protected readonly IUserProjectRepository _userProjectBaseRepository;
        public ProjectService(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IRepository<Project> projectRepository,
            IRepository<BankCard> bankCardBaseRepository,
            IRoleService roleService,
            IRepository<User> userRepository,
            IRepository<Role> roleRepository,
            IRepository<UserProject> userProjectRepository,
            UserManager<User> userManager,
            IProjectRepository projectBaseRepository,
            IUserProjectRepository userProjectBaseRepository
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
            _projectBaseRepository = projectBaseRepository;
            _userProjectRepository = userProjectRepository;
            _roleRepository = roleRepository;
            //_bankCardRepository = bankCardRepository;
        }

        public async Task CreateNewProjectAsync(ProjectCreateDTO projectDTO, string userId)
        {
            var user = await _userRepository.GetByKeyAsync(userId);

            if (user == null)
            {
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.UserNotFound);
            }

            var bankcard = _mapper.Map<BankCard>(projectDTO);
            var project = _mapper.Map<Project>(projectDTO);

            if (await _bankCardBaseRepository.GetByCardNumberAsync(projectDTO.CardNumber) == null)
            {
                project.BankCard = bankcard;
                await _bankCardBaseRepository.AddAsync(bankcard);
            }
            else
            {
                project.BankCard = await _bankCardBaseRepository.GetByCardNumberAsync(projectDTO.CardNumber);
            }

            await _projectRepository.AddAsync(project);
            await _projectRepository.SaveChangesAsync();

            var role = new Role
            {
                RoleName = "Owner",
                RolePoints = 100,
                Project = project

            };
            await _roleRepository.AddAsync(role);
            await _roleRepository.SaveChangesAsync();

            var userProject = new UserProject
            {
                IsOwner = true,
                MemberDate = DateTime.Now,
                PersonalPoints = 0,
                UserId = userId,
                Project = project,
                Role = role
            };

            await _userProjectRepository.AddAsync(userProject);
            await _userProjectRepository.SaveChangesAsync();

            await _userManager.AddToRoleAsync(user, "Owner");
            await _userManager.UpdateAsync(user);

            await _bankCardBaseRepository.SaveChangesAsync();
        }

        public async Task AddMemberToProjectAsync(string userId, int projectId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.UserNotFound);
            }

            var project = await _projectRepository.GetByKeyAsync(projectId);

            if (project == null)
            {
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.ProjectNotFound);
            }
            var role = new Role
            {
                RoleName = "Member",
                RolePoints = 100,
                Project = project

            };
            await _roleRepository.AddAsync(role);
            await _roleRepository.SaveChangesAsync();
            var userProject = new UserProject
            {
                IsOwner = false,
                MemberDate = DateTime.Now,
                PersonalPoints = 0,
                UserId = userId,
                Project = project,
                Role = role
            };

            await _userProjectRepository.AddAsync(userProject);
            await _userManager.AddToRoleAsync(user, "Member");
            await _userManager.UpdateAsync(user);
            await _userProjectRepository.SaveChangesAsync();
        }

        public async Task<ProjectInfoDTO> InfoFromProjectAsync(int projectId)
        {
            var projectObject = await _projectRepository.GetByKeyAsync(projectId);

            if (projectObject == null)
            {
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.ProjectNotFound);
            }

            var projectInfo = _mapper.Map<ProjectInfoDTO>(projectObject);

            return projectInfo;
        }

        public async Task EditProjectDateAsync(ProjectEditDTO projectEditDTO, int projectId)
        {
            var project = await _projectRepository.GetByKeyAsync(projectId);

            if (project == null)
            {
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.ProjectNotFound);
            }

            project.Name = projectEditDTO.Name;
            project.ProjectPoinPercent = projectEditDTO.ProjectPoinPercent;
            project.Currency = projectEditDTO.SelectedCurrency.ToString();
            project.SalaryDate = projectEditDTO.SalaryDate;
            project.BaseSalary = projectEditDTO.Salary;

            var bankCard = await _bankCardBaseRepository.GetBankCardByProjectIdAsync(projectId);

            if (bankCard == null)
            {
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.AttachmentNotFound);
            }

            bankCard.ExpirationDate = projectEditDTO.ExpirationDate;
            bankCard.CardNumber = projectEditDTO.NumberCard;
            bankCard.CardVerificationValue = projectEditDTO.CVV;
            bankCard.Money = projectEditDTO.Money;

            await _projectRepository.UpdateAsync(project);
            await _bankCardBaseRepository.UpdateAsync(bankCard);
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
            var userProjects = await _userProjectRepository.GetListAsync(up => up.UserId == userId && up.IsOwner);

            var projectIds = userProjects.Select(up => up.ProjectId);

            var projects = await _projectRepository.GetListAsync(p => projectIds.Contains(p.ProjectId));

            var projectDtos = _mapper.Map<IEnumerable<ProjectForUserDTO>>(projects);

            return projectDtos;
        }
    }
}
