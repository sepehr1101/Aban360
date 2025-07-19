using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Validations
{
    public class ContractualCapacityValidator:BaseValidator<ContractualCapacityInputDto>
    {
        public ContractualCapacityValidator()
        {
            RuleFor(customer => customer.FromDateJalali)
           .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
           .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(customer => customer.ToDateJalali)
           .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
           .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(input => input)
          .Must(input => FromToDateJalaliValidation.DateValidation(new FromToDateJalaliDto(input.FromDateJalali,
                                                                                          input.ToDateJalali)).IsValid)
          .WithMessage(input => FromToDateJalaliValidation.DateValidation(new FromToDateJalaliDto(input.FromDateJalali,
                                                                                          input.ToDateJalali)).ErrorMessage);

        }
    }
}
