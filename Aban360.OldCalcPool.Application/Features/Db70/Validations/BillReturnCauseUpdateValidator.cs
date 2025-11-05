using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Db70.Validations
{
    public class BillReturnCauseUpdateValidator : BaseValidator<BillReturnCauseUpdateDto>
    {
        public BillReturnCauseUpdateValidator()
        {
            RuleFor(v => v.Code)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(v => v.Title)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
