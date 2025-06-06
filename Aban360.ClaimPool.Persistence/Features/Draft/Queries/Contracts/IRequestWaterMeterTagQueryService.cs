﻿using Aban360.ClaimPool.Domain.Features.Draft.Entites;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts
{
    public interface IRequestWaterMeterTagQueryService
    {
        Task<RequestWaterMeterTag> Get(int id);
        Task<ICollection<RequestWaterMeterTag>> GetByWaterMeterId(int id);
        Task<ICollection<RequestWaterMeterTag>> Get();
    }
}
