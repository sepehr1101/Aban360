﻿using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.People.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts
{
    public interface IDiscountTypeGetSingleHandler
    {
        Task<DiscountTypeGetDto> Handle(DiscountTypeEnum id, CancellationToken cancellationToken);
    }
}
