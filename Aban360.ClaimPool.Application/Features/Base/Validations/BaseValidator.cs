using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Base.Validations
{
    public class BaseValidator<T>:AbstractValidator<T>
    {
        internal BaseValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
        }
    }
}
