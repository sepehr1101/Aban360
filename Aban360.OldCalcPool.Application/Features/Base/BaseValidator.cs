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
    }
}
