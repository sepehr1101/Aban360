using Aban360.Common.Literals;
using Aban360.OldCalcPool.Application.Features.Base;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Processing.Validations
{
    public class MeterChangeValidator : BaseValidator<MeterChangeInputDto>
    {
        public MeterChangeValidator()
        {
            RuleFor(g => g.BillId)
            .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
            .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(g => g.MeterChangeDateJalali)
                        .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                        .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(g => g.MeterNumber)
                        .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                        .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(g => g.BodySerial)
                        .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                        .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(g => g.ChangeCauseId)
                        .NotEmpty().WithMessage(ExceptionLiterals.EmptyString)
                        .NotNull().WithMessage(ExceptionLiterals.NotNull);

        }
    }
}
