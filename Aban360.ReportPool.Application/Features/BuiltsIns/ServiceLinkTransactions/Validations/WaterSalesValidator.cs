using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Validations
{
    public class WaterSalesValidator : BaseValidator<WaterSalesInputDto>
    {
        public WaterSalesValidator()
        {
            RuleFor(installation => installation.FromDateJalali)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(installation => installation.ToDateJalali)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(installation => installation.ZoneIds)
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
