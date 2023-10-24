using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Moneyboard.Core.DTO.UserDTO;
using Moneyboard.Core.Entities.UserEntity;

namespace Moneyboard.Core.Validation
{
    public class UserSetPasswordValidation : AbstractValidator<UserSetNewPasswordDTO>
    {
        protected readonly UserManager<User> _userManager;

        public UserSetPasswordValidation(UserManager<User> manager)
        {
            _userManager = manager;

            RuleFor(user => user.NewPassword)
                .NotEmpty()
                .MinimumLength(6)
                .Matches("[A-Z]")
                    .WithMessage("{PropertyName} must contain one or more capital letters.")
                .Matches("[a-z]")
                    .WithMessage("{PropertyName} must contain one or more lowercase letters.")
                .Matches(@"\d")
                    .WithMessage("{PropertyName} must contain one or more digits.")
                .Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]")
                    .WithMessage("{PropertyName} must contain one or more special characters.")
                .Matches("^[^£# “”]*$")
                    .WithMessage("{PropertyName} must not contain the following characters £ # “” or spaces.");
        }
    }
}
