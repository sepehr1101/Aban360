using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.WaterReturn.Validations
{
    public class OfferingToCreateRepairValidator : BaseValidator<OfferingToCreateRepairDto>
    {
        public OfferingToCreateRepairValidator()
        {
            //RuleFor(o => o.AbonFas)
            //    .NotNull().WithMessage(ExceptionLiterals.NotNull)
            //    .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            //RuleFor(o => o.FasBaha)
            //    .NotNull().WithMessage(ExceptionLiterals.NotNull)
            //    .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            //RuleFor(o => o.AbBaha)
            //    .NotNull().WithMessage(ExceptionLiterals.NotNull)
            //    .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            //RuleFor(o => o.Ztadil)
            //    .NotNull().WithMessage(ExceptionLiterals.NotNull)
            //    .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            //RuleFor(o => o.Shahrdari)
            //    .NotNull().WithMessage(ExceptionLiterals.NotNull)
            //    .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            //RuleFor(o => o.JalaseNo)
            //    .NotNull().WithMessage(ExceptionLiterals.NotNull)
            //    .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            //RuleFor(o => o.AbonAb)
            //    .NotNull().WithMessage(ExceptionLiterals.NotNull)
            //    .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(o => o.Elat)
                .NotNull().WithMessage(ExceptionLiterals.NotNull)
                .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            //RuleFor(o => o.ZaribFasl)
            //    .NotNull().WithMessage(ExceptionLiterals.NotNull)
            //    .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            //RuleFor(o => o.Ab10)
            //    .NotNull().WithMessage(ExceptionLiterals.NotNull)
            //    .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            //RuleFor(o => o.Ab20)
            //    .NotNull().WithMessage(ExceptionLiterals.NotNull)
            //    .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            //RuleFor(o => o.Zabresani)
            //    .NotNull().WithMessage(ExceptionLiterals.NotNull)
            //    .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            //RuleFor(o => o.ZaribD)
            //    .NotNull().WithMessage(ExceptionLiterals.NotNull)
            //    .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            //RuleFor(o => o.TabAbnA)
            //    .NotNull().WithMessage(ExceptionLiterals.NotNull)
            //    .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            //RuleFor(o => o.TabAbnF)
            //    .NotNull().WithMessage(ExceptionLiterals.NotNull)
            //    .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            //RuleFor(o => o.TabsFa)
            //    .NotNull().WithMessage(ExceptionLiterals.NotNull)
            //    .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            //RuleFor(o => o.Bodjeh)
            //    .NotNull().WithMessage(ExceptionLiterals.NotNull)
            //    .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            //RuleFor(o => o.Group1)
            //    .NotNull().WithMessage(ExceptionLiterals.NotNull)
            //    .NotEmpty().WithMessage(ExceptionLiterals.NotNull);

            //RuleFor(o => o.Faz)
            //    .NotNull().WithMessage(ExceptionLiterals.NotNull);

            //RuleFor(o => o.Avarez)
            //    .NotNull().WithMessage(ExceptionLiterals.NotNull)
            //    .NotEmpty().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
