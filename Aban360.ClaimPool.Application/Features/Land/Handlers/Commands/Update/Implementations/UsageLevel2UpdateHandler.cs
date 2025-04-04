﻿using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Implementations
{
    internal sealed class UsageLevel2UpdateHandler : IUsageLevel2UpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUsageLevel2QueryService _usageLevel2QueryService;
        public UsageLevel2UpdateHandler(
            IMapper mapper,
            IUsageLevel2QueryService usageLevel2QueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _usageLevel2QueryService = usageLevel2QueryService;
            _usageLevel2QueryService.NotNull(nameof(_usageLevel2QueryService));
        }

        public async Task Handle(UsageLevel2UpdateDto updateDto, CancellationToken cancellationToken)
        {
            var usageLevel2 = await _usageLevel2QueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, usageLevel2);
        }
    }
}
