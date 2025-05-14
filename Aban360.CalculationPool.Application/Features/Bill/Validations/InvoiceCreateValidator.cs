using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Aban360.CalculationPool.Application.Features.Bill.Validations
{
    public class InvoiceCreateValidator : BaseValidator<InvoiceCreateDto>
    {
        public InvoiceCreateValidator()
        {
            RuleFor(o => o.InvoiceTypeId)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(o => o.InvoiceStatusId)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(o => o.DepositRate)
                 .NotNull().WithMessage(ExceptionLiterals.NotNull)
                 .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

  

        }
    }
}
