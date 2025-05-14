using Aban360.BlobPool.Application.Features.Base;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Validations
{
    public class RequestWaterMeterUpdateValidator:BaseValidator<WaterMeterRequestUpdateDto>
    {
        public RequestWaterMeterUpdateValidator()
        {

            RuleFor(f => f.Id)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
            .NotNull().WithMessage(ExceptionLiterals.NotNull);

            RuleFor(f => f.BillId)
             .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
             .NotNull().WithMessage(ExceptionLiterals.NotNull)
             .MaximumLength(15).WithMessage(ExceptionLiterals.NotMoreThan15);

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


            RuleFor(f => f.CustomerNumber)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNull)
            .NotNull().WithMessage(ExceptionLiterals.NotNull);
        }
    }
}
