﻿using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    public class EstateUpdateHandler : IEstateUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IEstateQueryService _queryService;
        public EstateUpdateHandler(
            IMapper mapper,
           IEstateQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task Handle(EstateUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var estate = await _queryService.Get(updateDto.Id);
            if (estate == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, estate);
        }
    }
}
