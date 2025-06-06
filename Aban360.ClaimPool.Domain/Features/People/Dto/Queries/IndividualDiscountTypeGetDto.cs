﻿using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.ClaimPool.Domain.Features.People.Dto.Queries
{
    public record IndividualDiscountTypeGetDto
    {
        public int Id { get; set; }
        public int IndividualId { get; set; }
        public DiscountTypeEnum DiscountTypeId { get; set; }
        public Guid UserId { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
