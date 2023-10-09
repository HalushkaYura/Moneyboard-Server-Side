using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moneyboard.Core.DTO.ProjectDTO;
using Moneyboard.Core.DTO.RoleDTO;
using Moneyboard.Core.Entities.BankCardEntity;
using Moneyboard.Core.Entities.ProjectEntity;
using Moneyboard.Core.Entities.RoleEntity;
using Moneyboard.Core.Interfaces.Repository;
using Moneyboard.Core.Interfaces.Services;

namespace Moneyboard.Core.Services
{
    public class ProjectService : IProjectService
    {
        protected readonly IMapper _mapper;
        protected IRepository<Project> _projectRepository;
        protected IRepository<BankCard> _bankCardRepository;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProjectContext _projectContext;
        private readonly IRoleService _roleService;


        public ProjectService(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IRepository<Project> projectRepository,
            IRepository<BankCard> bankCardRepository,
            IProjectContext projectContext,
            IRoleService roleService)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _projectRepository = projectRepository;
            _bankCardRepository = bankCardRepository;
            _projectContext = projectContext;
            _roleService = roleService;
        }

        public async Task CreateProjectAsync(ProjectCreateDTO projectDTO)
        {


            if (await _bankCardRepository.GetByCardNumberAsync(projectDTO.NumberCard) == null)
            {
                var bankcard = _mapper.Map<BankCard>(projectDTO);
                var project = _mapper.Map<Project>(projectDTO);

                project.BankCard = bankcard;
                await _bankCardRepository.AddAsync(bankcard);
                await _projectRepository.AddAsync(project);
            }
            else
            {
                var project = _mapper.Map<Project>(projectDTO);
                project.BankCard = await _bankCardRepository.GetByCardNumberAsync(projectDTO.NumberCard);
                await _projectRepository.AddAsync(project);
            }


            await _projectRepository.SaveChangesAsync();
            await _bankCardRepository.SaveChangesAsync();

        }




    }
}
