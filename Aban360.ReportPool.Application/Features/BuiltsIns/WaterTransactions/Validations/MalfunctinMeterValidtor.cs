using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Application.Features.Base.Validations;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using FluentValidation;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Validations
{
    public class MalfunctinMeterValidtor : BaseValidator<MalfunctionMeterInputDto>
    {
        public MalfunctinMeterValidtor()
        {
            RuleFor(meter => meter.FromReadingNumber)
                .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(meter => meter.ToReadingNumber)
                .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);
        
            RuleFor(meter => meter.ZoneIds)
                .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
