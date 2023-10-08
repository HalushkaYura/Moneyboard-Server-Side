using FluentValidation;
using Moneyboard.Core.DTO.UserDTO;

namespace Moneyboard.Core.Validation
{
    public class UserTwoFactorLoginValidation : AbstractValidator<UserTwoFactorDTO>
    {
        public UserTwoFactorLoginValidation()
        {
            RuleFor(user => user.Token)
                .NotEmpty()
                .NotNull();

            RuleFor(user => user.Provider)
                .NotEmpty()
                .NotNull();

            RuleFor(user => user.Email)
                .NotEmpty()
                .NotNull();
        }
    }
}
