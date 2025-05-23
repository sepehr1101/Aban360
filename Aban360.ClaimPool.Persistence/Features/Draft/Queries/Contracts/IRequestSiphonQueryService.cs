﻿using Aban360.ClaimPool.Domain.Features.Draft.Entites;

namespace Aban360.ClaimPool.Persistence.Features.Draft.Queries.Contracts
{
    public interface IRequestSiphonQueryService
    {
        Task<RequestSiphon> Get(int id);
        Task<ICollection<RequestSiphon>> Get();
    }
}
