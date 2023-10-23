using FluentValidation;
using Moneyboard.Core.DTO.ProjectDTO;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyboard.Core.Validation
{
    public class PojectPointPercentValidator : AbstractValidator<ProjectPointDTO>
    {
        public PojectPointPercentValidator()
        {
            RuleFor(x => x.ProjectPoinPercent)
                .NotEmpty()
                .NotNull()
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(100);
        }
    }
}
