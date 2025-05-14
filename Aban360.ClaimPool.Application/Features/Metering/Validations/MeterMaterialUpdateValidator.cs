using Aban360.BlobPool.Application.Features.Base;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Metering.Validations
{
    public class MeterMaterialUpdateValidator : BaseValidator<MeterMaterialUpdateDto>
    {
        public MeterMaterialUpdateValidator()
        {
            RuleFor(f => f.Id)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.Title)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);
        }
    }
}
