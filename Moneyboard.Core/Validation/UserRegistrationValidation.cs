
using Moneyboard.Core.DTO.UserDTO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using Moneyboard.Core.Entities.UserEntity;
using FluentValidation;

namespace Moneyboard.Core.Validation
{
    public class UserRegistrationValidation : AbstractValidator<UserRegistrationDTO>
    {
        protected readonly UserManager<User> _userManager;

        public UserRegistrationValidation(UserManager<User> manager)
        {
            _userManager = manager;

            RuleFor(user => user.Firstname)
                .NotNull()
                .Length(3, 50);


            RuleFor(user => user.Lastname)
                .NotNull()
                .Length(3, 50);


            RuleFor(user => user.Email)
                .NotNull()
                .EmailAddress()
                .Must(IsUniqueUserEmail).WithMessage("{PropertyName} already exists.");

            RuleFor(user => user.Password)
                .NotEmpty()
                .MinimumLength(6)
                .Matches("[A-Z]").WithMessage("{PropertyName} must contain one or more capital letters.")
                .Matches("[a-z]").WithMessage("{PropertyName} must contain one or more lowercase letters.")
                .Matches(@"\d").WithMessage("{PropertyName} must contain one or more digits.")
                .Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]").WithMessage("{PropertyName} must contain one or more special characters.")
                .Matches("^[^£# “”]*$").WithMessage("{PropertyName} must not contain the following characters £ # “” or spaces.");
            
            RuleFor(user => user.CardNumber)
                .NotEmpty().WithMessage("{PropertyName} is not must empty.")
                .Matches("^[0-9]+$").WithMessage("{PropertyName} must contain only number.")
                .Matches(@"^\d{16}$").WithMessage("{PropertyName} must be exactly 16 digits.")
                .Must(IsValidLuhnAlgorithm).WithMessage("{PropertyName} is not a valid credit card number.");
        }

        private bool IsUniqueUserEmail(string email)
        {
            var userObject = _userManager.FindByEmailAsync(email).Result;
            return userObject == null;
        }

        private bool IsValidLuhnAlgorithm(string cardNumber)
        {
            int sum = 0;
            bool doubleDigit = false;

            // Alchoritm Luna
            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                int digit = int.Parse(cardNumber[i].ToString());

                if (doubleDigit)
                {
                    digit *= 2;
                    if (digit > 9)
                    {
                        digit -= 9;
                    }
                }

                sum += digit;
                doubleDigit = !doubleDigit;
            }

            return sum % 10 == 0;
        }
    }
}
