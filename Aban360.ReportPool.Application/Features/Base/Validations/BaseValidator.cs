using FluentValidation;

namespace Aban360.ReportPool.Application.Features.Base.Validations
{
    public class BaseValidator<T> : AbstractValidator<T>
    {
        public BaseValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
        }
    }
}
