using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Base;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Processing.Validations
{
    public class InstallmentValidator : BaseValidator<BillInstallmentInputDto>
    {
        public InstallmentValidator()
        {
            RuleFor(g => g.BillId)
            .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
            .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(g => g.InstallmentCount)
                .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .LessThanOrEqualTo(6).WithMessage(ExceptionLiterals.InvalidInstallmentMoreThan6);
        }
    }
}
