﻿using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Implementations
{
    internal sealed class UsageLevel3CreateHandler : IUsageLevel3CreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IUsageLevel3CommandService _usageLevel3CommandService;
        public UsageLevel3CreateHandler(
            IMapper mapper,
            IUsageLevel3CommandService usageLevel3CommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _usageLevel3CommandService = usageLevel3CommandService;
            _usageLevel3CommandService.NotNull(nameof(_usageLevel3CommandService));
        }

        public async Task Handle(UsageLevel3CreateDto createDto, CancellationToken cancellationToken)
        {
            var usageLevel3 = _mapper.Map<UsageLevel3>(createDto);
            await _usageLevel3CommandService.Add(usageLevel3);
        }
    }
}
