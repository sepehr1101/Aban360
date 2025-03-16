﻿using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Implemntations
{
    internal sealed class UsageGetSingleHandler : IUsageGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IUsageQuerySevice _usageQueryService;
        public UsageGetSingleHandler(
            IMapper mapper,
           IUsageQuerySevice usageQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _usageQueryService = usageQueryService;
            _usageQueryService.NotNull(nameof(usageQueryService));
        }

        public async Task<UsageGetDto> Handle(short id,CancellationToken cancellationToken)
        {
            Usage usage = await _usageQueryService.Get(id);
            if (usage == null)
            {
                throw new InvalidDataException();
            }
            return _mapper.Map<UsageGetDto>(usage);
        }
    }
}
