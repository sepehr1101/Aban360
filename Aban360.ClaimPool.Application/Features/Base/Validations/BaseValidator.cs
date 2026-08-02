using Aban360.Common.Literals;
using Aban360.Common.Timing;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Base.Validations
{
    public class BaseValidator<T> : AbstractValidator<T>
    {
        internal BaseValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
        }
        protected virtual bool IsDigit(string input) => !string.IsNullOrWhiteSpace(input) && input.All(char.IsDigit);
        protected virtual bool IsValidMobileNumber(string input) =>
            !string.IsNullOrEmpty(input) &&
            input.Length == 11 &&
            input.StartsWith("09") &&
            IsDigit(input);
        protected virtual bool IsValidMobileNumberOrNull(string? input) =>
            string.IsNullOrEmpty(input) ||
            (input.Length == 11 &&
            input.StartsWith("09") &&
            IsDigit(input));
        protected virtual bool IsValidPhoneNumber(string? input) =>
            string.IsNullOrWhiteSpace(input)
            || ((input.Length == 8 || input.Length == 11) && IsDigit(input));
        protected virtual bool IsValidNationalCode(string input) =>
            !string.IsNullOrEmpty(input) &&
            input.Length == 10 &&
            IsDigit(input);
        protected virtual bool IsValidNullableNationalCode(string? input) =>
            string.IsNullOrWhiteSpace(input) ||
            (input.Length == 10 && IsDigit(input));
        protected virtual bool IsValidPostalCode(string input) =>
            !string.IsNullOrEmpty(input) &&
            input.Length == 10 &&
            IsDigit(input);
        protected virtual bool IsValidGuid(string input) => Guid.TryParse(input, out _);
        protected virtual bool IsValidInt(string input) => int.TryParse(input, out _);
        protected virtual bool IsValidDateJalali(string input) =>
            !string.IsNullOrEmpty(input) &&
            input.Length >= 10 &&
            input.All(c => char.IsDigit(c) || c == '/');
        protected virtual bool IsValidNullableDateJalali(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return true;
            }
            string gregorianData = ConvertDate.JalaliToGregorian(input);
            if (gregorianData == ExceptionLiterals.Incalculable)
            {
                return false;
            }
            return true;
        }
    }
}
