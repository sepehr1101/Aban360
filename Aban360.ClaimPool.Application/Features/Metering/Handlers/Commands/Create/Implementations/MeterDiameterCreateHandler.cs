﻿using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Persistence.Features.Metering.Commands.Contracts;
using Aban360.Common.Extensions;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Implementations
{
    public class MeterDiameterCreateHandler : IMeterDiameterCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IMeterDiameterCommandService _meterDiameterCommandService;
        public MeterDiameterCreateHandler(
            IMapper mapper,
            IMeterDiameterCommandService meterDiameterCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _meterDiameterCommandService = meterDiameterCommandService;
            _meterDiameterCommandService.NotNull(nameof(meterDiameterCommandService));
        }

        public async Task Handle(MeterDiameterCreateDto createDto, CancellationToken cancellationToken)
        {
            var meterDiameter = _mapper.Map<MeterDiameter>(createDto);
            await _meterDiameterCommandService.Add(meterDiameter);
        }
    }
}

