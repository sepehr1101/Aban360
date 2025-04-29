using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Bill.Validations
{
    public abstract class BaseValidator<T>:AbstractValidator<T>
    {
        public BaseValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
        }
    }
}
