using FluentValidation;
using Moneyboard.Core.DTO.UserDTO;

namespace Moneyboard.Core.Validation
{
    public class UserLogValidation : AbstractValidator<UserLoginDTO>
    {
        public UserLogValidation()
        {
            RuleFor(user => user.Email)
                .NotEmpty()
                .NotNull();

            RuleFor(user => user.Password)
                .NotEmpty()
                .NotNull();
        }
    }
}
