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

            RuleFor(dto => dto.BaseSalary)
                .GreaterThan(0);

            RuleFor(dto => dto.SalaryDay)
                .NotEmpty()
                .GreaterThan(0)
                .LessThan(28);

            RuleFor(dto => dto.Currency)
                .IsInEnum();
        }

    }
}
