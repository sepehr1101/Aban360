﻿namespace Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands
{
    public record SiphonDiameterUpdateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
