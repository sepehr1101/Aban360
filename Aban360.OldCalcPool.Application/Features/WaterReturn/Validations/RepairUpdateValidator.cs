using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.WaterReturn.Validations
{
    public class RepairUpdateValidator : BaseValidator<RepairUpdateDto>
    {
        public RepairUpdateValidator()
        {
            RuleFor(o => o.Id)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(o => o.JalaseNo)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(o => o.Elat)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
