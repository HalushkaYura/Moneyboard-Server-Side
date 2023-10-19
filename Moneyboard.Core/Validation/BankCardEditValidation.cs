using FluentValidation;
using Moneyboard.Core.DTO.BankCardDTO;

namespace Moneyboard.Core.Validation
{
    public class BankCardEditValidation : AbstractValidator<BankCardEditDTO>
    {
        public BankCardEditValidation()
        {
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
