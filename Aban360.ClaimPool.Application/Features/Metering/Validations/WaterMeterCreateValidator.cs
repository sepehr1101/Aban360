using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Metering.Validations
{
    public class WaterMeterCreateValidator : BaseValidator<WaterMeterCreateDto>
    {
        public WaterMeterCreateValidator()
        {
            RuleFor(f => f.BillId)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull)
             .MaximumLength(13).WithMessage(ExceptionLiterals.NotMoreThan13)
             .MinimumLength(6).WithMessage(ExceptionLiterals.NotLessThan6);

            RuleFor(f => f.UseStateId)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .IsInEnum().WithMessage(ExceptionLiterals.MustEnum);

            RuleFor(f => f.SubscriptionTypeId)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
              .NotNull().WithMessage(ExceptionLiterals.NotNull)
              .IsInEnum().WithMessage(ExceptionLiterals.MustEnum);

            RuleFor(f => f.MeterDiameterId)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
            .NotNull().WithMessage(ExceptionLiterals.NotNull)
            .GreaterThan((short)0).WithMessage(ExceptionLiterals.GreaterThan0);

            RuleFor(f => f.MeterProducerId)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
            .NotNull().WithMessage(ExceptionLiterals.NotNull)
             .GreaterThan((short)0).WithMessage(ExceptionLiterals.GreaterThan0);


            RuleFor(f => f.MeterTypeId)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
            .NotNull().WithMessage(ExceptionLiterals.NotNull)
             .GreaterThan((short)0).WithMessage(ExceptionLiterals.GreaterThan0);


            RuleFor(f => f.MeterMaterialId)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
            .NotNull().WithMessage(ExceptionLiterals.NotNull)
            .GreaterThan((short)0).WithMessage(ExceptionLiterals.GreaterThan0);


            RuleFor(f => f.MeterUseTypeId)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
            .NotNull().WithMessage(ExceptionLiterals.NotNull)
            .GreaterThan((short)0).WithMessage(ExceptionLiterals.GreaterThan0);
        }
    }
}
