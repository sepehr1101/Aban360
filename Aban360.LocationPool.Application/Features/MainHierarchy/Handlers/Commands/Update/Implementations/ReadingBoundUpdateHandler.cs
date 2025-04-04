﻿using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Implementations
{
    internal sealed class ReadingBoundUpdateHandler : IReadingBoundUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IReadingBoundQueryService _readingBoundQueryService;
        public ReadingBoundUpdateHandler(
           IMapper mapper,
            IReadingBoundQueryService readingBoundQueryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _readingBoundQueryService = readingBoundQueryService;
            _readingBoundQueryService.NotNull(nameof(readingBoundQueryService));
        }

        public async Task Handle(ReadingBoundUpdateDto updateDto, CancellationToken cancellationToken)
        {
            ReadingBound readingBound = await _readingBoundQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, readingBound);
        }
    }
}
