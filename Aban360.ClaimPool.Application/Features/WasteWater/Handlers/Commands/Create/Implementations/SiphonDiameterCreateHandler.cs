﻿using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Commands.Contracts;
using AutoMapper;
using Aban360.Common.Extensions;
using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Implementations
{
    internal sealed class SiphonDiameterCreateHandler : ISiphonDiameterCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly ISiphonDiameterCommandService _commandService;
        public SiphonDiameterCreateHandler(
            IMapper mapper,
            ISiphonDiameterCommandService commandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _commandService = commandService;
            _commandService.NotNull(nameof(commandService));
        }

        public async Task Handle(SiphonDiameterCreateDto createDto, CancellationToken cancellationToken)
        {
            SiphonDiameter SiphonDiameter = _mapper.Map<SiphonDiameter>(createDto);
            await _commandService.Add(SiphonDiameter);
        }
    }
}
