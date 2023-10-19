﻿using FluentValidation;
using Moneyboard.Core.DTO.ProjectDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            RuleFor(dto => dto.SalaryDay)
                .NotEmpty()
                .GreaterThan(0)
                .LessThan(28);

            RuleFor(dto => dto.CardNumber)
                .NotEmpty()
                .Matches(@"^\d{16}$")
                .Matches("^[0-9]+$")
                .Must(IsValidLuhnAlgorithm).WithMessage("{PropertyName} is not a valid credit card number.");
            

            RuleFor(dto => dto.CardVerificationValue)
                .NotEmpty()
                .Length(3);

            RuleFor(dto => dto.ExpirationDate)
                .NotEmpty();


            RuleFor(dto => dto.Currency)
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
