using Aban360.Common.Literals;
using Aban360.InstallationPool.Application.Features.Base;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using FluentValidation;

namespace Aban360.InstallationPool.Application.Features.Definition.Validations
{
    public class EquipmentBrokerUpdateValidator : BaseValidator<EquipmentBrokerUpdateDto>
    {
        public EquipmentBrokerUpdateValidator()
        {
            RuleFor(f => f.Id)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.Title)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

            RuleFor(f => f.ApiUrl)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .MaximumLength(1023).WithMessage(ExceptionLiterals.NotMoreThan1023);

            RuleFor(f => f.Website)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .MaximumLength(1023).WithMessage(ExceptionLiterals.NotMoreThan1023);
        }
    }
}
