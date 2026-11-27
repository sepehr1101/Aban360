using FluentValidation;

namespace Aban360.MeterPool.Application.Features.Base.Validations
{
    public class BaseValidator<T> : AbstractValidator<T>
    {
        internal BaseValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
        }
    }
}
