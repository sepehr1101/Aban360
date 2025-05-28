using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Validations
{
    public class RequestIndividualUpdateValidator:BaseValidator<IndividualRequestUpdateDto>
    {
        public RequestIndividualUpdateValidator()
        {
            RuleFor(f => f.Id)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull);
            
            RuleFor(f => f.IndividualTypeId)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.FirstName)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

            RuleFor(f => f.Surname)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

            RuleFor(f => f.NationalId)
                .Length(10).WithMessage(ExceptionLiterals.Equal10);

            RuleFor(f => f.FatherName)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

            RuleFor(f => f.PhoneNumbers)
               .MaximumLength(1023).WithMessage(ExceptionLiterals.NotMoreThan1023);

            RuleFor(f => f.MobileNumbers)
               .MaximumLength(1023).WithMessage(ExceptionLiterals.NotMoreThan1023);

            RuleFor(f => f.IndividualEstateRelationTypeId)
               .IsInEnum().WithMessage(ExceptionLiterals.MustEnum);
        }
    
    }
}
