﻿namespace Aban360.ClaimPool.Domain.Features.People.Dto.Queries
{
    public record IndividualTagGetDto
    {
        public int Id { get; set; }
        public int IndividualId { get; set; }
        public short IndividualTagDefinitionId { get; set; }
        public string? Value { get; set; }
    }
}
