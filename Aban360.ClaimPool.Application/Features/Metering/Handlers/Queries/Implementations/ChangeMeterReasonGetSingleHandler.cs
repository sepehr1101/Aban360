﻿using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Implementations
{
    internal sealed class ChangeMeterReasonGetSingleHandler : IChangeMeterReasonGetSingleHandler
    {
        private readonly IMapper _mapper;
        private readonly IChangeMeterReasonQueryService _changeMeterReasonQueryService;
        public ChangeMeterReasonGetSingleHandler(
            IMapper mapper,
            IChangeMeterReasonQueryService changeMeterReasonQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _changeMeterReasonQueryService = changeMeterReasonQueryService;
            _changeMeterReasonQueryService.NotNull(nameof(_changeMeterReasonQueryService));
        }

        public async Task<ChangeMeterReasonGetDto> Handle(ChangeMeterReasonEnum id, CancellationToken cancellationToken)
        {
            ChangeMeterReason changeMeterReason = await _changeMeterReasonQueryService.Get(id);
            return _mapper.Map<ChangeMeterReasonGetDto>(changeMeterReason);
        }
    }
}
