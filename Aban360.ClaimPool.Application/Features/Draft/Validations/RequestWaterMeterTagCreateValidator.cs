using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Validations
{
    public class RequestWaterMeterTagCreateValidator : BaseValidator<WaterMeterTagRequestCreateDto>
    {
        public RequestWaterMeterTagCreateValidator()
        {
            RuleFor(s => s.WaterMeterTagDefinitionId)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(s => s.WaterMeterId)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull);

        }
    }
}