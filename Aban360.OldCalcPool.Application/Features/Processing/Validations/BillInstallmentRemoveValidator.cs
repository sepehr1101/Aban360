using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Processing.Validations
{
    public class BillInstallmentRemoveValidator : BaseValidator<BillInstallmentRemoveInputDto>
    {
        public BillInstallmentRemoveValidator()
        {
            RuleFor(b => b.BillId)
                .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(b => b.ZoneId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(b => b.CustomerNumber)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(b => b.Id)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
