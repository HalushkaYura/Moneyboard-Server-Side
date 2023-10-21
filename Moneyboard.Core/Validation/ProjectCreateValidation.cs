using FluentValidation;
using Moneyboard.Core.DTO.ProjectDTO;

namespace Moneyboard.Core.Validation
{
    public class ProjectCreateValidation : AbstractValidator<ProjectCreateDTO>
    {
        public ProjectCreateValidation()
        {
            RuleFor(dto => dto.Name)
                 .NotEmpty()
                 .MaximumLength(255);

            RuleFor(dto => dto.BaseSalary)
                .GreaterThanOrEqualTo(0);

            RuleFor(dto => dto.Money)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(dto => dto.SalaryDay)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0)
                .LessThan(28);

            RuleFor(dto => dto.CardNumber)
                .NotEmpty()
                .NotNull()
                .Matches(@"^\d{16}$")
                .Matches("^[0-9]+$")
                .Must(IsValidLuhnAlgorithm).WithMessage("{PropertyName} is not a valid credit card number.");


            RuleFor(dto => dto.CardVerificationValue)
                .NotEmpty()
                .NotNull()
                .Length(3);

            RuleFor(dto => dto.ExpirationDate)
                .NotNull()
                .NotEmpty();


            RuleFor(dto => dto.Currency)
                .NotNull()
               .NotEmpty();
        }

        private bool IsValidLuhnAlgorithm(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber) || !cardNumber.All(char.IsDigit))
            {
                return true;
            }
            int sum = 0;
            bool doubleDigit = false;

            // Luhn Algorithm
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
