using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Validations
{
    public class RequestFlatUpdateValidator : BaseValidator<FlatRequestUpdateDto>
    {
        public RequestFlatUpdateValidator()
        {
            RuleFor(f => f.Id)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.Storey)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
                .NotNull().WithMessage(ExceptionLiterals.NotNull);

        }
    }
}
