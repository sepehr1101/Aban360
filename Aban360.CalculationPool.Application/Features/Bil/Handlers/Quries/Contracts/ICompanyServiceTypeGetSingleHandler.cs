﻿using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;

namespace Aban360.CalculationPool.Application.Features.Bil.Handlers.Quries.Contracts
{
    public interface ICompanyServiceTypeGetSingleHandler
    {
        Task<CompanyServiceTypeGetDto> Handle(short id, CancellationToken cancellationToken);
    }
}
