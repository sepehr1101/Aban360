﻿using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts
{
    public interface IRequestWaterMeterTagCreateHandler
    {
        Task Handle(WaterMeterTagRequestCreateDto createDto, CancellationToken cancellationToken);
    }
}
