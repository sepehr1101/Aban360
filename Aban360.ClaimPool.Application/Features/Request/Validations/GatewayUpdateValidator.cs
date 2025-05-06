using Aban360.BlobPool.Application.Features.Base;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Request.Validations
{
    public class GatewayUpdateValidator : BaseValidator<GatewayUpdateDto>
    {
        public GatewayUpdateValidator()
        {
            RuleFor(f => f.Id)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
             .NotNull().WithMessage(ExceptionLiterals.NotNUll);

            RuleFor(f => f.Title)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
               .NotNull().WithMessage(ExceptionLiterals.NotNUll)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);
        }
    }
}
