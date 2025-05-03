using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Validations
{
    public partial class RequestFlatCreateValidator : BaseValidator<FlatRequestCreateDto>
    {
        public RequestFlatCreateValidator()
        {
            RuleFor(f => f.Storey)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
                .NotNull().WithMessage(ExceptionLiterals.NotNUll);

        }
    }
}