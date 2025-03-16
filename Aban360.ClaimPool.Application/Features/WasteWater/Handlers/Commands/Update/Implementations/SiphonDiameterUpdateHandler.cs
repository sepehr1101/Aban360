﻿using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts;
using AutoMapper;
using Aban360.Common.Extensions;
using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Implementations
{
    internal sealed class SiphonDiameterUpdateHandler : ISiphonDiameterUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly ISiphonDiameterQueryService _queryService;
        public SiphonDiameterUpdateHandler(
            IMapper mapper,
            ISiphonDiameterQueryService queryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _queryService = queryService;
            _queryService.NotNull(nameof(queryService));
        }

        public async Task Handle(SiphonDiameterUpdateDto updateDto, CancellationToken cancellationToken)
        {
            SiphonDiameter siphonDiameter = await _queryService.Get(updateDto.Id);
            _mapper.Map(updateDto, siphonDiameter);
        }
    }
}
