using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Processing.Validations
{
    public class MeterComparisonBatchValidator : BaseValidator<MeterComparisonBatchInputDto>
    {
        public MeterComparisonBatchValidator()
        {
            RuleFor(input => input.ZoneId)
                .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(input => input.FromDateJalali)
                 .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                 .NotNull().WithMessage(ExceptionLiterals.NotNull)
                 .Must(MoreThan1403).WithMessage(ExceptionLiterals.FromDateMoreThanDate(ExceptionLiterals.Date1403_01_01));


            RuleFor(input => input.ToDateJalali)
                 .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                 .NotNull().WithMessage(ExceptionLiterals.NotNull)
                 .Must(MoreThan1403).WithMessage(ExceptionLiterals.ToDateMoreThanDate(ExceptionLiterals.Date1403_01_01));

            RuleFor(input => input)
                .Must(ChechPercent).WithMessage(ExceptionLiterals.InvalidPercent);
        }
        private bool MoreThan1403(string date)
        {
            string date1403_01_01 = ExceptionLiterals.Date1403_01_01;
            return date.CompareTo(date1403_01_01) >= 0;
        }
        private bool ChechPercent(MeterComparisonBatchInputDto input)
        {
            if (input.IsPercent)
                return input.Tolerance <= 100 || input.Tolerance >= 0;

            return true;
        }
    }
}
