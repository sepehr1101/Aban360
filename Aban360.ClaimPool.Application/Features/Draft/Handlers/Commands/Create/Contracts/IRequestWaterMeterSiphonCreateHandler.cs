﻿using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts
{
    public interface IRequestWaterMeterSiphonCreateHandler
    {
        Task Handle(WaterMeterSiphonRequestCreateDto createDto, CancellationToken cancellationToken);
    }
}
