using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Base
{
    public class BaseValidator<T> : AbstractValidator<T>
    {
        public BaseValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;
        }
        protected virtual bool IsDigit(string input) => !string.IsNullOrWhiteSpace(input) && input.All(char.IsDigit);
        protected virtual bool IsValidMobileNumber(string input) =>
           !string.IsNullOrEmpty(input) &&
           input.Length == 11 &&
           input.StartsWith("09") &&
           IsDigit(input);
    }
}
