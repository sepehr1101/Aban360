using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Contracts;
using Aban360.LocationPool.Domain.Features.MainHierarchy;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Implementations
{
    public class ReadingBoundCreateHandler : IReadingBoundCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IReadingBoundCommandService _readingBoundCommandService;
        public ReadingBoundCreateHandler(
            IMapper mapper,
            IReadingBoundCommandService readingBoundCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _readingBoundCommandService = readingBoundCommandService;
            _readingBoundCommandService.NotNull(nameof(readingBoundCommandService));
        }

        public async Task Handle(ReadingBoundCreateDto createDto, CancellationToken cancellationToken)
        {
            var readingBound = _mapper.Map<ReadingBound>(createDto);
            await _readingBoundCommandService.Add(readingBound);
        }
    }
}
