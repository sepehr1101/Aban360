using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Bill.Validations
{
    public class CompanyServiceOfferingUpdateValidator : BaseValidator<CompanyServiceOfferingUpdateDto>
    {
        public CompanyServiceOfferingUpdateValidator()
        {
            RuleFor(o => o.Id)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(o => o.CompanyServiceId)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(o => o.OfferingId)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
