using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Validations
{
    public class MalfunctionToChangeValidator : BaseValidator<MalfunctionToChangeInputDto>
    {
        public MalfunctionToChangeValidator()
        {
            RuleFor(input => input)
              .Must(input => FromToDateJalaliValidation.DateValidation(new FromToDateJalaliDto(input.FromDateJalali,
                                                                                              input.ToDateJalali)).IsValid)
              .WithMessage(input => FromToDateJalaliValidation.DateValidation(new FromToDateJalaliDto(input.FromDateJalali,
                                                                                                     input.ToDateJalali)).ErrorMessage);

        }
    }
}
