﻿using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Metering.Validations
{
    public class WaterMeterUpdateValidator : AbstractValidator<WaterMeterUpdateDto>
    {
        public WaterMeterUpdateValidator()
        {
            RuleFor(f => f.Id)
            .NotEmpty().WithMessage("Not Empty")
            .NotNull().WithMessage("Not Nyull");

            RuleFor(f => f.UseStateId)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull")
              .IsInEnum().WithMessage("SubscriptionTypeId must Enum");

            RuleFor(f => f.SubscriptionTypeId)
              .NotEmpty().WithMessage("Not Empty")
              .NotNull().WithMessage("Not Nyull")
              .IsInEnum().WithMessage("SubscriptionTypeId must Enum");

            RuleFor(f => f.MeterDiameterId)
            .NotEmpty().WithMessage("Not Empty")
            .NotNull().WithMessage("Not Nyull")
            .GreaterThan((short)0).WithMessage("MeterDiameterId not Equal 0");

            RuleFor(f => f.MeterProducerId)
            .NotEmpty().WithMessage("Not Empty")
            .NotNull().WithMessage("Not Nyull")
             .GreaterThan((short)0).WithMessage("MeterProducerId not Equal 0");


            RuleFor(f => f.MeterTypeId)
            .NotEmpty().WithMessage("Not Empty")
            .NotNull().WithMessage("Not Nyull")
             .GreaterThan((short)0).WithMessage("MeterTypeId not Equal 0");


            RuleFor(f => f.MeterMaterialId)
            .NotEmpty().WithMessage("Not Empty")
            .NotNull().WithMessage("Not Nyull")
            .GreaterThan((short)0).WithMessage("MeterMaterialId not Equal 0");


            RuleFor(f => f.MeterUseTypeId)
            .NotEmpty().WithMessage("Not Empty")
            .NotNull().WithMessage("Not Nyull")
            .GreaterThan((short)0).WithMessage("MeterUseTypeId not Equal 0");

        }
    }
}
