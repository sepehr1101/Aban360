﻿using Aban360.ClaimPool.Domain.Features.Metering.Entities;

namespace Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands
{
    public record MeterTypeGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public virtual ICollection<WaterMeter> WaterMeters { get; set; } = new List<WaterMeter>();
    }
}
