﻿using Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.People.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Persistence.Features.People.Commands.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.People.Handlers.Commands.Create.Implementations
{
    public class IndividualTagDefinitionCreateHandler : IIndividualTagDefinitionCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IIndividualTagDefinitionCommandService _commandService;
        private readonly IIndividualTagDefinitionQueryService _queryService;
        public IndividualTagDefinitionCreateHandler(
            IMapper mapper,
            IIndividualTagDefinitionCommandService commandService,
            IIndividualTagDefinitionQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task Handle(IndividualTagDefinitionCreateDto createDto, CancellationToken cancellationToken)
        {
            var individualTagDefinition = _mapper.Map<IndividualTagDefinition>(createDto);
            await _commandService.Add(individualTagDefinition);
        }
    }
}
