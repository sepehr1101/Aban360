using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Rule.Validations
{
    public class BaseValidator<T>:AbstractValidator<T>
    {
        public BaseValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
        }
    }
}
