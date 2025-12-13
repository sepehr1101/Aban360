using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Processing.Validations
{
    public class ManualBillValidator : BaseValidator<ManualBillInputDto>
    {
        public ManualBillValidator()
        {
            RuleFor(input => input.FromDateJalali)
                 .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                 .NotNull().WithMessage(ExceptionLiterals.NotNull);


            RuleFor(input => input.ToDateJalali)
                 .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                 .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
