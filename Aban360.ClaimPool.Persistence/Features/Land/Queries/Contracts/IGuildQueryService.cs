﻿using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IGuildQueryService
    {
        Task<Guild> Get(short id);
        Task<ICollection<Guild>> Get();
    }
}
