﻿using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Update.Implementations
{
    internal sealed class IndividualTagDefinitionUpdateHandler : IIndividualTagDefinitionUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualTagDefinitionQueryService _queryService;
        public IndividualTagDefinitionUpdateHandler(
            IMapper mapper,
            IIndividualTagDefinitionQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task Handle(IndividualTagDefinitionUpdateDto updateDto, CancellationToken cancellationToken)
        {
            IndividualTagDefinition individualTagDefinition = await _queryService.Get(updateDto.Id);
            _mapper.Map(updateDto, individualTagDefinition);
        }
    }
}
