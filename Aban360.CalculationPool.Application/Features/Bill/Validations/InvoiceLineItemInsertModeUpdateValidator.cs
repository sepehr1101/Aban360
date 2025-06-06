﻿using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Bill.Validations
{
    public class InvoiceLineItemInsertModeUpdateValidator : BaseValidator<InvoiceLineItemInsertModeUpdateDto>
    {
        public InvoiceLineItemInsertModeUpdateValidator()
        {
            RuleFor(o => o.Id)
               .IsInEnum().WithMessage(ExceptionLiterals.MustEnum)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(o => o.Title)
               .NotNull().WithMessage(ExceptionLiterals.NotNull)
               .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan255);

            RuleFor(o => o.Description)
               .MaximumLength(255).WithMessage(ExceptionLiterals.NotMoreThan1023);
        }
    }
}
