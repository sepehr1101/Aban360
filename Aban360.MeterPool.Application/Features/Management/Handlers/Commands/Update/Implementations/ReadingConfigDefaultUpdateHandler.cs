﻿using Aban360.Common.Extensions;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Update.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Features.Management.Queries.Contracts;
using AutoMapper;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Update.Implementations
{
    internal sealed class ReadingConfigDefaultUpdateHandler : IReadingConfigDefaultUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IReadingConfigDefaultQueryService _readingConfigDefaultQueryService;
        private readonly IHeadquartersAddhoc _headquarterAddhoc;
        public ReadingConfigDefaultUpdateHandler(
            IMapper mapper,
            IReadingConfigDefaultQueryService readingConfigDefaultQueryService,
            IHeadquartersAddhoc headquarterAddhoc)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _readingConfigDefaultQueryService = readingConfigDefaultQueryService;
            _readingConfigDefaultQueryService.NotNull(nameof(_readingConfigDefaultQueryService));

            _headquarterAddhoc = headquarterAddhoc;
            _headquarterAddhoc.NotNull(nameof(_headquarterAddhoc));
        }

        public async Task Handle(ReadingConfigDefaultUpdateDto updateDto, CancellationToken cancellationToken)
        {
            ReadingConfigDefault readingConfigDefault = await _readingConfigDefaultQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, readingConfigDefault);
            var headquarterTitle = await _headquarterAddhoc.Handle(updateDto.HeadquartersId, cancellationToken);
            readingConfigDefault.HeadquartersTitle = headquarterTitle;
        }
    }
}
