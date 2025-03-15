using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Implementations
{
    internal sealed class ReadingBlockUpdateHandler : IReadingBlockUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IReadingBlockQeryService _readingBlockQeryService;
        public ReadingBlockUpdateHandler(
            IMapper mapper,
           IReadingBlockQeryService readingBlockQeryService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _readingBlockQeryService = readingBlockQeryService;
            _readingBlockQeryService.NotNull(nameof(readingBlockQeryService));
        }

        public async Task Handle(ReadingBlockUpdateDto updateDto, CancellationToken cancellationToken)
        {
            ReadingBlock readingBlock = await _readingBlockQeryService.Get(updateDto.Id);
            _mapper.Map(updateDto, readingBlock);
        }
    }
}
