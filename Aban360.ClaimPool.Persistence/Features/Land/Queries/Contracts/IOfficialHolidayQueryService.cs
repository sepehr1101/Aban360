﻿using Aban360.ClaimPool.Domain.Features.Land.Entities;

namespace Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts
{
    public interface IOfficialHolidayQueryService
    {
        Task<OfficialHoliday> Get(short id);
        Task<ICollection<OfficialHoliday>> Get();
    }
}
