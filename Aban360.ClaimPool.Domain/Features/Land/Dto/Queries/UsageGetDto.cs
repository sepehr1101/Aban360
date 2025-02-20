﻿namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record UsageGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public short ProvinceId { get; set; }
    }
}
