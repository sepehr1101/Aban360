using Aban360.CalculationPool.Application.Features.Bill.Validations;
using Aban360.CalculationPool.Domain.Features.WaterReturn.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.WaterReturn.Validations
{
    public class RepairDeleteValidator : BaseValidator<RepairDeleteDto>
    {
        public RepairDeleteValidator()
        {
            RuleFor(o => o.Id)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
