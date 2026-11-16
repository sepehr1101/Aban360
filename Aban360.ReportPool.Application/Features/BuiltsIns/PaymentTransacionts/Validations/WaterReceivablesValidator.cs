using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using DNTPersianUtils.Core;
using FluentValidation;
namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Validations;
public sealed class WaterReceivablesValidator : AbstractValidator<WaterReceivablesInputDto>
{
    public WaterReceivablesValidator()
    {
        RuleFor(x => x.GroupBy).IsInEnum().WithMessage("نوع گروه‌بندی نامعتبر است.");
        RuleFor(x => x.FromDateJalali).Must(IsValidDate).WithMessage("تاریخ شروع نامعتبر است.");
        RuleFor(x => x.ToDateJalali).Must(IsValidDate).WithMessage("تاریخ پایان نامعتبر است.");
        RuleFor(x => x).Must(x => string.CompareOrdinal(x.FromDateJalali, x.ToDateJalali) <= 0)
            .When(x => IsValidDate(x.FromDateJalali) && IsValidDate(x.ToDateJalali))
            .WithMessage("تاریخ پایان باید بزرگ‌تر یا مساوی تاریخ شروع باشد.");
        RuleFor(x => x.FromDateJalali).Must(x => string.CompareOrdinal(x, "1330/01/01") >= 0)
            .When(x => IsValidDate(x.FromDateJalali))
            .WithMessage("تاریخ شروع نباید قبل از 1330/01/01 باشد.");
    }

    private static bool IsValidDate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || !System.Text.RegularExpressions.Regex.IsMatch(value, @"^[0-9]{4}/[0-9]{2}/[0-9]{2}$"))
            return false;
        try { return value.ToGregorianDateOnly().HasValue; }
        catch (ArgumentException) { return false; }
        catch (FormatException) { return false; }
    }
}
