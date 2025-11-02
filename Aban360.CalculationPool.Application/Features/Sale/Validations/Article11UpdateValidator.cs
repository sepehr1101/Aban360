using Aban360.CalculationPool.Application.Features.Bill.Validations;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Sale.Validations
{
    public class Article11UpdateValidator : BaseValidator<Article11UpdateDto>
    {
        public Article11UpdateValidator()
        {
            RuleFor(i => i.Id)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.DomesticWaterAmount)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.NonDomesticWaterAmount)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.FromDateJalali)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.ToDateJalali)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(i => i.ZoneId)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

        }
    }
}
