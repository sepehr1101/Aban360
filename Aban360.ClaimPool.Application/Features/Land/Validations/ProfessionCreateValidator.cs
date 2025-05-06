using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Validations
{
    public class ProfessionCreateValidator : BaseValidator<ProfessionCreateDto>
    {
        public ProfessionCreateValidator()
        {
            RuleFor(f => f.Id)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
             .NotNull().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(f => f.Title)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

            RuleFor(f => f.GuildId)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(f => f.Description)
                .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);
        }
    }
}