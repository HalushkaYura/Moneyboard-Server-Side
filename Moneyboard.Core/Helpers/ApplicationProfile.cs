using AutoMapper;
using Moneyboard.Core.ApiModels;
using Moneyboard.Core.DTO.ProjectDTO;
using Moneyboard.Core.DTO.RoleDTO;
using Moneyboard.Core.DTO.UserDTO;
using Moneyboard.Core.Entities.BankCardEntity;
using Moneyboard.Core.Entities.ProjectEntity;
using Moneyboard.Core.Entities.RoleEntity;
using Moneyboard.Core.Entities.UserEntity;

namespace Moneyboard.Core.Helpers
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {


            CreateMap<User, UserChangeInfoDTO>()
                .ForMember(dest => dest.Email, act => act.MapFrom(src => src.Email))
                .ForMember(dest => dest.Firstname, act => act.MapFrom(src => src.Firstname))
                .ForMember(dest => dest.Lastname, act => act.MapFrom(src => src.Lastname))
                .ForMember(dest => dest.CardNumber, act => act.MapFrom(src => src.CardNumber))
                .ForMember(dest => dest.BirstDate, act => act.MapFrom(src => src.BirthDate));

            /*CreateMap<InviteUser, UserInviteInfoDTO>()
                .ForMember(x => x.Id, act => act.MapFrom(srs => srs.Id))
                .ForMember(x => x.Date, act => act.MapFrom(srs => srs.Date))
                .ForMember(x => x.IsConfirm, act => act.MapFrom(srs => srs.IsConfirm))
                .ForMember(x => x.WorkspaceName, act => act.MapFrom(srs => srs.Workspace.Name))
                .ForMember(x => x.FromUserName, act => act.MapFrom(srs => srs.FromUser.Name))
                .ForMember(x => x.ToUserId, act => act.MapFrom(srs => srs.ToUserId));*/

            CreateMap<UserRegistrationDTO, User>()
                 .ForMember(dest => dest.Email, act => act.MapFrom(src => src.Email))
                 .ForMember(dest => dest.Firstname, act => act.MapFrom(src => src.Firstname))
                 .ForMember(dest => dest.Lastname, act => act.MapFrom(src => src.Lastname))
                 .ForMember(dest => dest.CardNumber, act => act.MapFrom(src => src.CardNumber));

            CreateMap<ProjectCreateDTO, BankCard>()
                 .ForMember(dest => dest.CardNumber, act => act.MapFrom(src => src.NumberCard))
                 .ForMember(dest => dest.CardVerificationValue, act => act.MapFrom(src => src.CVV))
                 .ForMember(dest => dest.ExpirationDate, act => act.MapFrom(src => src.ExpirationDate))
                 .ForMember(dest => dest.Money, act => act.MapFrom(src => src.Money));

            CreateMap<ProjectCreateDTO, Project>()
                 .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name))
                 .ForMember(dest => dest.Currency, act => act.MapFrom(src => src.SelectedCurrency))
                 .ForMember(dest => dest.BaseSalary, act => act.MapFrom(src => src.Salary))
                 .ForMember(dest => dest.ProjectPoinPercent, act => act.MapFrom(src => src.ProjectPoinPercent))
                 .ForMember(dest => dest.SalaryDate, act => act.MapFrom(src => src.SalaryDate));

            CreateMap<Project, ProjectInfoDTO>()
                .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name))
                .ForMember(dest => dest.Currency, act => act.MapFrom(src => src.Currency))
                .ForMember(dest => dest.Salary, act => act.MapFrom(src => src.BaseSalary))
                .ForMember(dest => dest.SalaryDate, act => act.MapFrom(src => src.SalaryDate));

            CreateMap<RoleCreateDTO, Role>()
              .ForMember(dest => dest.RoleName, act => act.MapFrom(src => src.Name))
              .ForMember(dest => dest.RolePoints, act => act.MapFrom(src => src.RolePoints));




            /* CreateMap<BlobDownloadInfo, DownloadFile>()
                 .ForMember(x => x.ContentType, act => act.MapFrom(srs => srs.Details.ContentType))
                 .ForMember(x => x.Content, act => act.MapFrom(srs => srs.Content));*/

        }
    }
}
