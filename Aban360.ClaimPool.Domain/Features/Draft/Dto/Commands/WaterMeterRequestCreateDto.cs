﻿using Aban360.ClaimPool.Domain.Features._Base.Dto;

namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands
{
    public record WaterMeterRequestCreateDto : WaterMeterCommandBaseDto
    {
        public int EstateId { get; set; }
    }
}
