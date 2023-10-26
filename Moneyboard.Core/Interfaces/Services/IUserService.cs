using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moneyboard.Core.ApiModels;
using Moneyboard.Core.DTO.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyboard.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserChangeInfoDTO> UserInfoAsync(string userId);
        Task EditUserDateAsync(UserEditDTO userEditDTO, string userId);
        //Task<List<UserInviteInfoDTO>> GetUserInviteInfoListAsync(string userId);
        //Task<UserActiveInviteDTO> IsActiveInviteAsync(string userId);
        Task ChangeTwoFactorVerificationStatusAsync(string userId, UserChange2faStatusDTO statusDTO);
        Task<bool> CheckIsTwoFactorVerificationAsync(string userId);
        Task SendTwoFactorCodeAsync(string userId);
        Task SetPasswordAsync(string userId, UserSetPasswordDTO userSetPasswordDTO);
        Task<bool> IsHavePasswordAsync(string userId);

        //----------------------------------------------------------
        Task UploadAvatar(UserImageUploadDTO imageDTO, string userId);
        Task<string> GetUserImageAsync(string userId);
        //Task<byte[]> GetUserImageAsync(string email);

        Task DeleteUserAccount(string userId);
    }
}
