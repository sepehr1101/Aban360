using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Db70.Validations
{
    public class SearchShortInputValidator : BaseValidator<SearchShortInputDto>
    {
        public SearchShortInputValidator()
        {
            RuleFor(v => v.Id)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

        }
    }
}
