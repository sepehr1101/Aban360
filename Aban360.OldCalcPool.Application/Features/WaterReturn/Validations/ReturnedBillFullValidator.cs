using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Validations
{
    public class ReturnedBillFullValidator : BaseValidator<ReturnBillFullInputDto>
    {
        public ReturnedBillFullValidator()
        {
            RuleFor(r => r.BillId)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(r => r.FromDateJalali)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(r => r.ToDateJalali)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(r => r.ReturnCauseId)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);
            
        }
    }
}
