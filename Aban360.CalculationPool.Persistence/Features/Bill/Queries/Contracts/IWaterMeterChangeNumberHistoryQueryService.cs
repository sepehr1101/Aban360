﻿using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts
{
    public interface IWaterMeterChangeNumberHistoryQueryService
    {
        Task<WaterMeterChangeNumberHistory> Get(long id);
        Task<ICollection<WaterMeterChangeNumberHistory>> Get();
    }
}
