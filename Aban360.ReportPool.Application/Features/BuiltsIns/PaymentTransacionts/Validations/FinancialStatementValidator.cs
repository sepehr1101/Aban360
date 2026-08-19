using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Validations
{
    public class FinancialStatementValidator : BaseValidator<FinancialStatementInputDto>
    {
        public FinancialStatementValidator()
        {
            RuleFor(input => input)
               .Must(input => FromToDateJalaliValidation.DateValidation(new FromToDateJalaliDto(input.FromDateJalali,
                                                                                            input.ToDateJalali)).IsValid)
               .WithMessage(input => FromToDateJalaliValidation.DateValidation(new FromToDateJalaliDto(input.FromDateJalali,
                                                                                            input.ToDateJalali)).ErrorMessage);
        }
    }
}
