using FluentValidation;
using Moneyboard.Core.DTO.ProjectDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyboard.Core.Validation
{
    public class ProjectEditValidation : AbstractValidator<ProjectEditDTO>
    {
        public ProjectEditValidation()
        {
            RuleFor(dto => dto.Name)
                 .NotEmpty()
                 .MaximumLength(255);

            RuleFor(dto => dto.Salary)
                .GreaterThanOrEqualTo(6400);

            RuleFor(dto => dto.SalaryDate)
                .NotEmpty();

            RuleFor(dto => dto.NumberCard)
                .NotEmpty()
                .Matches(@"^\d{16}$")
                .Matches("^[0-9]+$")
                .Must(IsValidLuhnAlgorithm).WithMessage("{PropertyName} is not a valid credit card number.");
            ;

            RuleFor(dto => dto.CVV)
                .NotEmpty()
                .Length(3);

            RuleFor(dto => dto.ExpirationDate)
                .NotEmpty();

            RuleFor(dto => dto.Money)
                .GreaterThanOrEqualTo(10000);

            RuleFor(dto => dto.SelectedCurrency)
                .IsInEnum();
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
