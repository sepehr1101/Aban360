using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Land.Validations
{
    public class EstateWaterResourceCreateValidator : BaseValidator<EstateWaterResourceCreateDto>
    {
        public EstateWaterResourceCreateValidator()
        {
            RuleFor(f => f.EstateId)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.WaterResourceId)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}