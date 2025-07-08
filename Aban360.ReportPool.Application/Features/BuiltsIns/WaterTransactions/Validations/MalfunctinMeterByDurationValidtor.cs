using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Validations
{
    public class MalfunctinMeterByDurationValidtor : BaseValidator<MalfunctionMeterByDurationInputDto>
    {
        public MalfunctinMeterByDurationValidtor()
        {
            RuleFor(MeterByDuration => MeterByDuration.FromReadingNumber)
                .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(MeterByDuration => MeterByDuration.ToReadingNumber)
                .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);
        
            RuleFor(MeterByDuration => MeterByDuration.ZoneIds)
                .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
