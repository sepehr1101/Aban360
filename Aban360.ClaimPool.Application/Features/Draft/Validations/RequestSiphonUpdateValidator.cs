using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Validations
{
    public class RequestSiphonUpdateValidator : BaseValidator<SiphonRequestUpdateDto>
    {
        public RequestSiphonUpdateValidator()
        {
            RuleFor(s => s.Id)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(s => s.InstallationLocation)
            .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

            RuleFor(s => s.InstallationDate)
                .Length(10).WithMessage(ExceptionLiterals.Equal10);

            RuleFor(s => s.SiphonDiameterId)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(s => s.SiphonTypeId)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(s => s.SiphonMaterialId)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}