using AutoMapper;
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

            CreateMap<User, UserChangeInfoDTO>().ReverseMap();
            CreateMap<UserRegistrationDTO, User>().ReverseMap();
            CreateMap<ProjectCreateDTO, BankCard>().ReverseMap();
            CreateMap<ProjectCreateDTO, Project>().ReverseMap();
            CreateMap<Project, ProjectInfoDTO>().ReverseMap();
            CreateMap<RoleCreateDTO, Role>().ReverseMap();
            CreateMap<ProjectForUserDTO, Project>().ReverseMap();



            /*CreateMap<InviteUser, UserInviteInfoDTO>()
                .ForMember(x => x.Id, act => act.MapFrom(srs => srs.Id))
                .ForMember(x => x.Date, act => act.MapFrom(srs => srs.Date))
                .ForMember(x => x.IsConfirm, act => act.MapFrom(srs => srs.IsConfirm))
                .ForMember(x => x.WorkspaceName, act => act.MapFrom(srs => srs.Workspace.Name))
                .ForMember(x => x.FromUserName, act => act.MapFrom(srs => srs.FromUser.Name))
                .ForMember(x => x.ToUserId, act => act.MapFrom(srs => srs.ToUserId));*/
    

            /* CreateMap<BlobDownloadInfo, DownloadFile>()
                 .ForMember(x => x.ContentType, act => act.MapFrom(srs => srs.Details.ContentType))
                 .ForMember(x => x.Content, act => act.MapFrom(srs => srs.Content));*/

        }
    }
}
