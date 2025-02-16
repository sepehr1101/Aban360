﻿using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Queries.Contracts
{
    public interface IIndividualTagDefinitionGetAllHandler
    {
        Task<ICollection<IndividualTagDefinitionGetDto>> Handle(CancellationToken cancellationToken);
    }
}
