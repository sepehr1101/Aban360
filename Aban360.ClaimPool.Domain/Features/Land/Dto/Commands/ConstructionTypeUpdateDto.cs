﻿namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record ConstructionTypeUpdateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
