using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.ReportPool.Domain.Features.BuiltIns.Sms.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.Sms.Validations
{
    public class GetCustomerMobileValidator : BaseValidator<GetCustomerMobileInputDto>
    {
        public GetCustomerMobileValidator()
        {
            RuleFor(b => b.BillId)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
