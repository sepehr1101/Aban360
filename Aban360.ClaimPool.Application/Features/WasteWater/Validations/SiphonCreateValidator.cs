﻿using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Validations
{
    public class SiphonCreateValidator:AbstractValidator<SiphonCreateDto>
    {
        public SiphonCreateValidator()
        {
            RuleFor(s => s.SiphonDiameterId)
                           .NotEmpty().WithMessage("Not Empty")
                           .NotNull().WithMessage("Not Nyull");

            RuleFor(s => s.SiphonTypeId)
                .NotEmpty().WithMessage("Not Empty")
                .NotNull().WithMessage("Not Nyull");

            RuleFor(s => s.SiphonMaterialId)
               .NotEmpty().WithMessage("Not Empty")
               .NotNull().WithMessage("Not Nyull");
        }
    }
}
