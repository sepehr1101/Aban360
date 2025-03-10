﻿using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;

namespace Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts
{
    public interface ICompanyServiceOfferingUpdateHandler
    {
        Task Handle(CompanyServiceOfferingUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
