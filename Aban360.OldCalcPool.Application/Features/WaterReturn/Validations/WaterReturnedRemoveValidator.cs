using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Validations
{
    public class WaterReturnedRemoveValidator : BaseValidator<WaterReturedRemoveInputDto>
    {
        public WaterReturnedRemoveValidator()
        {
            RuleFor(r => r.Id)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(r => r.ZoneId)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(r => r.CustomerNumber)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(r => r.BillId)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
