﻿using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;

namespace Aban360.ReportPool.Persistence.Features.ConsumersInfo.Contracts
{
    public interface IChangeMainInfoService
    {
        Task<IEnumerable<ChangeMainInfoDto>> GetInfo(string billId);
    }
}
