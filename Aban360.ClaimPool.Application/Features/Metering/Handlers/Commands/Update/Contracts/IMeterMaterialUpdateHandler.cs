﻿using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts
{
    public interface IMeterMaterialUpdateHandler
    {
        Task Handle(MeterMaterialUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
