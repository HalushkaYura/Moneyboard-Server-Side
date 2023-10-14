using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moneyboard.Core.DTO.ProjectDTO;
using Moneyboard.Core.Entities.BankCardEntity;
using Moneyboard.Core.Entities.ProjectEntity;
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
        protected IRepository<Project> _projectRepository;
        protected IRepository<BankCard> _bankCardBaseRepository;
        //protected IBankCardRepository _bankCardRepository;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IRoleService _roleService;
        protected readonly IRepository<UserProject> _userProjectRepository;
        protected readonly IRepository<User> _userRepository;
        public ProjectService(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IRepository<Project> projectRepository,
            IRepository<BankCard> bankCardBaseRepository,
            IRoleService roleService,
            IRepository<User> userRepository,
            IRepository<UserProject> userProjectRepository)
            //IBankCardRepository bankCardRepository)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _projectRepository = projectRepository;
            _bankCardBaseRepository = bankCardBaseRepository;
            _roleService = roleService;
            _userProjectRepository = userProjectRepository;
            _userRepository = userRepository;
            //_bankCardRepository = bankCardRepository;
        }

        public async Task CreateNewProjectAsync(ProjectCreateDTO projectDTO, string userId)
        {
            var user = await _userRepository.GetByKeyAsync(userId); // Отримуємо користувача за ідентифікатором

            if (user == null)
            {
                // Обробка помилки, якщо користувача не знайдено
                throw new HttpException(System.Net.HttpStatusCode.BadRequest,
                                  ErrorMessages.UserNotFound);
            }

            if (await _bankCardBaseRepository.GetByCardNumberAsync(projectDTO.NumberCard) == null)
            {
                var bankcard = _mapper.Map<BankCard>(projectDTO);
                var project = _mapper.Map<Project>(projectDTO);

                project.BankCard = bankcard;
                await _bankCardBaseRepository.AddAsync(bankcard);
                await _projectRepository.AddAsync(project);

                // Створюємо об'єкт UserProject та прив'язуємо його до користувача та проекту
                var userProject = new UserProject
                {
                    IsOwner = true, // Позначаємо користувача як власника проекту
                    MemberDate = DateTime.Now,
                    PersonalPoints = 0, // Якщо потрібно вказати якісь особисті бали користувача
                    UserId = userId, // ID користувача, який створив проект
                    Project = project // Прив'язуємо до проекту
                };

                await _userProjectRepository.AddAsync(userProject);
            }
            else
            {
                var project = _mapper.Map<Project>(projectDTO);
                project.BankCard = await _bankCardBaseRepository.GetByCardNumberAsync(projectDTO.NumberCard);
                await _projectRepository.AddAsync(project);

                // Створюємо об'єкт UserProject та прив'язуємо його до користувача та проекту
                var userProject = new UserProject
                {
                    IsOwner = true, // Позначаємо користувача як власника проекту
                    MemberDate = DateTime.Now,
                    PersonalPoints = 0, // Якщо потрібно вказати якісь особисті бали користувача
                    UserId = userId, // ID користувача, який створив проект
                    Project = project // Прив'язуємо до проекту
                };

                await _userProjectRepository.AddAsync(userProject);
            }

            await _projectRepository.SaveChangesAsync();
            await _bankCardBaseRepository.SaveChangesAsync();
            await _userProjectRepository.SaveChangesAsync();
        }


        public async Task<ProjectInfoDTO> InfoFromProjectAsync(int projectId)
        {

            var projectObject = await _projectRepository.GetByKeyAsync(projectId);
            if (projectObject == null)
            {
                throw new HttpException(System.Net.HttpStatusCode.BadRequest,
                  ErrorMessages.ProjectNotFound);
            }
            var projectInfo = _mapper.Map<ProjectInfoDTO>(projectObject);

            return projectInfo;

        }

        private async Task<bool> IsProjectOwnedByUserAsync(string userId, int projectId)
        {
            // Перевіряємо, чи користувач є власником проекту
            var userProject = await _userProjectRepository.GetByPairOfKeysAsync(userId, projectId);
            return userProject != null && userProject.IsOwner;
        }

        public async Task EditProjectDateAsync(ProjectEditDTO projectEditDTO, int projectId)
        {
            var project = await _projectRepository.GetByKeyAsync(projectId);
            if (project == null)
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.ProjectNotFound);

            project.Name = projectEditDTO.Name;
            project.ProjectPoinPercent = projectEditDTO.ProjectPoinPercent;
            project.Currency = projectEditDTO.SelectedCurrency.ToString();
            project.SalaryDate = projectEditDTO.SalaryDate;
            project.BaseSalary = projectEditDTO.Salary;

            var bankCard = await _bankCardBaseRepository.GetBankCardByProjectIdAsync(projectId);
            if (bankCard == null)
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.AttachmentNotFound);

            bankCard.ExpirationDate = projectEditDTO.ExpirationDate;
            bankCard.CardNumber = projectEditDTO.NumberCard;
            bankCard.CardVerificationValue = projectEditDTO.CVV;
            bankCard.Money = projectEditDTO.Money;


            await _projectRepository.UpdateAsync(project);
            await _bankCardBaseRepository.UpdateAsync(bankCard);

        }

        public Task<IEnumerable<Project>> AllUserProjectAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
