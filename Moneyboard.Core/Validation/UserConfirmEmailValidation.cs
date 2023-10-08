using FluentValidation;
using Moneyboard.Core.DTO.UserDTO;

namespace Moneyboard.Core.Validation
{
    public class UserConfirmEmailValidation : AbstractValidator<UserConfirmEmailDTO>
    {
        public UserConfirmEmailValidation()
        {
            RuleFor(email => email.ConfirmationCode)
                .NotEmpty()
                .NotNull();
        }
    }
}
