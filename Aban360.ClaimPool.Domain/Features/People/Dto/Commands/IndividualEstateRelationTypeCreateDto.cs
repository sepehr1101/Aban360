﻿using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.ClaimPool.Domain.Features.People.Dto.Commands
{
    public record IndividualEstateRelationTypeCreateDto
    {
        public IndividualEstateRelationEnum Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
