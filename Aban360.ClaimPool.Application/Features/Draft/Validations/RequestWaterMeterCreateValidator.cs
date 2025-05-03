using Aban360.ClaimPool.Application.Features.Base.Validations;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Draft.Validations
{
    public class RequestWaterMeterCreateValidator : BaseValidator<WaterMeterRequestCreateDto>
    {
        public RequestWaterMeterCreateValidator()
        {

            RuleFor(f => f.BillId)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
            .NotNull().WithMessage(ExceptionLiterals.NotNUll)
            .MaximumLength(15).WithMessage(ExceptionLiterals.NotMoreThan15);

            RuleFor(f => f.UseStateId)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
              .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .IsInEnum().WithMessage(ExceptionLiterals.MustEnum);

            RuleFor(f => f.SubscriptionTypeId)
              .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
              .NotNull().WithMessage(ExceptionLiterals.NotNUll)
              .IsInEnum().WithMessage(ExceptionLiterals.MustEnum);

            RuleFor(f => f.MeterDiameterId)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
            .NotNull().WithMessage(ExceptionLiterals.NotNUll)
            .GreaterThan((short)0).WithMessage(ExceptionLiterals.GreaterThan0);

            RuleFor(f => f.MeterProducerId)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
            .NotNull().WithMessage(ExceptionLiterals.NotNUll)
             .GreaterThan((short)0).WithMessage(ExceptionLiterals.GreaterThan0);


            RuleFor(f => f.MeterTypeId)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
            .NotNull().WithMessage(ExceptionLiterals.NotNUll)
             .GreaterThan((short)0).WithMessage(ExceptionLiterals.GreaterThan0);


            RuleFor(f => f.MeterMaterialId)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
            .NotNull().WithMessage(ExceptionLiterals.NotNUll)
            .GreaterThan((short)0).WithMessage(ExceptionLiterals.GreaterThan0);


            RuleFor(f => f.MeterUseTypeId)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
            .NotNull().WithMessage(ExceptionLiterals.NotNUll)
            .GreaterThan((short)0).WithMessage(ExceptionLiterals.GreaterThan0);


            RuleFor(f => f.CustomerNumber)
            .NotEmpty().WithMessage(ExceptionLiterals.NotNUll)
            .NotNull().WithMessage(ExceptionLiterals.NotNUll);
        }
    }
}
