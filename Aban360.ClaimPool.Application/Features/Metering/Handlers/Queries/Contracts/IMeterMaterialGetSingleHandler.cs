﻿using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts
{
    public interface IMeterMaterialGetSingleHandler
    {
        Task<MeterMaterialGetDto> Handle(short id, CancellationToken cancellationToken);
    }
}
