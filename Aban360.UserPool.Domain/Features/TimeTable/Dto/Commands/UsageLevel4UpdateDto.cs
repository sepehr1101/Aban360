﻿namespace Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands
{
    public record UsageLevel4UpdateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public short UsageLevel3Id { get; set; }

    }
}
