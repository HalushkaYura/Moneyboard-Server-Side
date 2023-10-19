using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moneyboard.Core.DTO.BankCardDTO;
using Moneyboard.Core.Entities.BankCardEntity;
using Moneyboard.Core.Entities.ProjectEntity;
using Moneyboard.Core.Entities.UserProjectEntity;
using Moneyboard.Core.Exeptions;
using Moneyboard.Core.Interfaces.Repository;
using Moneyboard.Core.Interfaces.Services;
using Moneyboard.Core.Resources;

namespace Moneyboard.Core.Services
{
    public class BanckCardService : IBankCardService
    {
        protected readonly IMapper _mapper;
        protected IRepository<BankCard> _bankCardBaseRepository;
        //protected IBankCardRepository _bankCardRepository;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IRepository<UserProject> _userProjectRepository;
        protected IRepository<Project> _projectRepository;

        public BanckCardService(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IRepository<BankCard> bankCardBaseRepository,
            IRoleService roleService,
            IRepository<UserProject> userProjectRepository,
            IRepository<Project> projectRepository
                                                    //IProjectRepository projectBaseRepository,
                                                    //IUserProjectRepository userProjectBaseRepository
                                                    //IBankCardRepository bankCardRepository
                                                    )
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _bankCardBaseRepository = bankCardBaseRepository;
            _userProjectRepository = userProjectRepository;
            _projectRepository = projectRepository;
            //_projectBaseRepository = projectBaseRepository;
            //_userProjectRepository = userProjectBaseRepository;
            //_bankCardRepository = bankCardRepository;
        }

        public async Task EditBankCardDateAsync(BankCardEditDTO banckCardEditDTO, int projectId, string userId)
        {
            var project = await _projectRepository.GetByKeyAsync(projectId );

            var bankCard = await _bankCardBaseRepository.GetBankCardByProjectIdAsync(projectId);

            if (bankCard == null)
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.AttachmentNotFound);

            _mapper.Map(banckCardEditDTO, bankCard);
            bankCard.Money = project.BaseSalary * 2;

            await _bankCardBaseRepository.UpdateAsync(bankCard);
            await _bankCardBaseRepository.SaveChangesAsync();
        }

        public async Task<BankCardInfoDTO> InfoBankCardAsync(int projectId, string userId)
        {
            var userProject = await _userProjectRepository.GetListAsync(up => up.ProjectId == projectId && up.UserId == userId);
            if (userProject == null || userProject.First().IsOwner == false)
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.AttachmentNotFound);

            var bankCard = await _bankCardBaseRepository.GetBankCardByProjectIdAsync(projectId);
            if (bankCard == null)
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.AttachmentNotFound);

            var bankCardInfo = _mapper.Map<BankCardInfoDTO>(bankCard);
            return bankCardInfo;
        }
    }
}
