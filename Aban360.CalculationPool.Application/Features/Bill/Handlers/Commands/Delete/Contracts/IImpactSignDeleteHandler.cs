﻿using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Contracts
{
    public interface IImpactSignDeleteHandler
    {
        Task Handle(ImpactSignDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
