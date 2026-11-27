using Aban360.MeterPool.Domain.Features.AutoReading.Dtos.Queries;
using FluentValidation;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Aban360.MeterPool.Application.Features.AutoReading.Validations
{
    public sealed class FlowMeterReportValidator : AbstractValidator<FlowMeterReportInputDto>
    {
        public FlowMeterReportValidator()
        {
            RuleFor(x => x.FromDate).Must(IsValidPersianDate)
                .WithMessage("تاریخ شروع باید تاریخ شمسی معتبر با قالب yyyy/MM/dd باشد.");
            RuleFor(x => x.ToDate).Must(IsValidPersianDate)
                .WithMessage("تاریخ پایان باید تاریخ شمسی معتبر با قالب yyyy/MM/dd باشد.");
            RuleFor(x => x.ToDate)
                .Must((input, toDate) => string.CompareOrdinal(input.FromDate, toDate) <= 0)
                .When(x => IsValidPersianDate(x.FromDate) && IsValidPersianDate(x.ToDate))
                .WithMessage("تاریخ پایان نباید قبل از تاریخ شروع باشد.");
        }

        private static bool IsValidPersianDate(string? value)
        {
            if (value is null || !Regex.IsMatch(value, @"\A[0-9]{4}/[0-9]{2}/[0-9]{2}\z"))
                return false;

            try
            {
                var parts = value.Split('/');
                new PersianCalendar().ToDateTime(
                    int.Parse(parts[0], CultureInfo.InvariantCulture),
                    int.Parse(parts[1], CultureInfo.InvariantCulture),
                    int.Parse(parts[2], CultureInfo.InvariantCulture), 0, 0, 0, 0);
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }
    }
}
