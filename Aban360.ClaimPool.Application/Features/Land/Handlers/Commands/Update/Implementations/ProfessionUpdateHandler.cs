﻿using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class ProfessionUpdateHandler : IProfessionUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IProfessionQueryService _queryService;
        public ProfessionUpdateHandler(
            IMapper mapper,
            IProfessionQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task Handle(ProfessionUpdateDto updateDto, CancellationToken cancellationToken)
        {
            Profession profession = await _queryService.Get(updateDto.Id);
            _mapper.Map(updateDto, profession);
        }
    }
}
