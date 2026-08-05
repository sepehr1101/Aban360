using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Validations
{
    public class ConsumptionManagementByBillIdValidator : BaseValidator<ConsumptionManagementByBillIdInputDto>
    {
        public ConsumptionManagementByBillIdValidator()
        {
            RuleFor(c => c.BillId)
                .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(payment => payment)
            .Must(input => DateValidate(input.FromDateJalali, input.ToDateJalali).IsValid).WithMessage(input => DateValidate(input.FromDateJalali, input.ToDateJalali).ErrorMessage);

            RuleFor(c => c.FromDateJalali)
                .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(c => c.ToDateJalali)
                .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
