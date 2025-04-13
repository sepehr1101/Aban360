using FluentValidation;

namespace Aban360.UserPool.Application.Features.Auth.Validations
{
    public  abstract class BaseValidator<T>:AbstractValidator<T>
    {
        public BaseValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
        }
    }
}
