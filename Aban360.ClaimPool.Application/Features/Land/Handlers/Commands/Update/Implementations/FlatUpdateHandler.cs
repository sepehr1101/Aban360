﻿using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    public class FlatUpdateHandler : IFlatUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IFlatQueryService _queryService;
        public FlatUpdateHandler(
            IMapper mapper,
            IFlatQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task Handle(FlatUpdateDto updateDto, CancellationToken cancellationToken)
        {
            var flat = await _queryService.Get(updateDto.Id);
            if (flat == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, flat);
        }
    }
}
