﻿using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;

namespace Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Contracts
{
    public interface ITariffDuplicateHandler
    {
        Task Handle(TariffDuplicateDto createDto, CancellationToken cancellationToken);
    }
}
