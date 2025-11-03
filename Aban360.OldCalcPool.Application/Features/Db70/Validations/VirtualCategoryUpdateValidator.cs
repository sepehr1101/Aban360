using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Db70.Validations
{
    public class VirtualCategoryUpdateValidator : BaseValidator<VirtualCategoryUpdateDto>
    {
        public VirtualCategoryUpdateValidator()
        {
            RuleFor(v => v.Id)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(v => v.Code)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(v => v.Title)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(v => v.Multiplier)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .LessThan(100).WithMessage(ExceptionLiterals.NotMoreThan100);

        }
    }
}
