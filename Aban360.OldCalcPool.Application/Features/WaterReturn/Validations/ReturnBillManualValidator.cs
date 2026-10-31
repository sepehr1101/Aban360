using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Validations
{
    public class ReturnBillManualValidator : BaseValidator<ReturnBillManualInputDto>
    {
        public ReturnBillManualValidator()
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
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(r => r.ReturnType)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(r => r.Item1)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(r => r.Item2)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(r => r.Item3)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(r => r.Item4)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(r => r.Item5)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(r => r.Item6)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(r => r.Item10)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(r => r.Item11)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(r => r.Item13)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(r => r.Item14)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(r => r.Item16)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
