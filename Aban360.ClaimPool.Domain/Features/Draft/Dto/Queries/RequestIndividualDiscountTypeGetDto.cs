﻿using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries
{
    public record RequestIndividualDiscountTypeGetDto
    {
        public int Id { get; set; }
        public DiscountTypeEnum DiscountTypeId { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
