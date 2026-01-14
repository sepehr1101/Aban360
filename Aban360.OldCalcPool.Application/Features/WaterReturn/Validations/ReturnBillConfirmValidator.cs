using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Validations
{
    public class ReturnBillConfirmValidator : BaseValidator<ReturnBillConfirmeByBillIdInputDto>
    {
        public ReturnBillConfirmValidator()
        {
            RuleFor(r => r.BillId)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(r => r.MinutesNumber)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
