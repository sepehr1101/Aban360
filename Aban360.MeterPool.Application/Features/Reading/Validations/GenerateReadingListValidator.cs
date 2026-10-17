using Aban360.Common.Literals;
using Aban360.MeterPool.Application.Features.Base.Validations;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using FluentValidation;

namespace Aban360.MeterPool.Application.Features.Reading.Validations
{
    public class GenerateReadingListValidator : BaseValidator<ReadingCreateDto>
    {
        public GenerateReadingListValidator()
        {
            RuleFor(e => e.ReadingBoundId)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull);
            
            RuleFor(e => e.ReadingBlockInfo)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull);
            
            RuleFor(e => e.Year)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull);
            
            RuleFor(e => e.ReadingPeriodTypeId)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull);
            
            RuleFor(e => e.ReadingPeriodId)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull);

        }
    }
}
