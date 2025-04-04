﻿namespace Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands
{
    public record ZoneCreateDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int RegionId { get; set; }
        public string UnstandardCode { get; set; } //= null!;
    }
}
