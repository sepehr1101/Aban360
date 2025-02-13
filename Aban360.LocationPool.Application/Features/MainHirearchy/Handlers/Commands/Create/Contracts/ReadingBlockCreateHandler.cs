using Aban360.Common.Extensions;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;
using Aban360.LocationPool.Persistence.Features.MainHirearchy.Commands.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Create.Contracts
{
    public class ReadingBlockCreateHandler : IReadingBlockCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IReadingBlockCommandService _readingBlockCommandService;
        public ReadingBlockCreateHandler(
            IMapper mapper,
            IReadingBlockCommandService readingBlockCommandService)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(mapper));

            _readingBlockCommandService = readingBlockCommandService;
            _readingBlockCommandService.NotNull(nameof(readingBlockCommandService));
        }

        public async Task Handle(ReadingBlockCreateDto createDto, CancellationToken cancellationToken)
        {
            var readingBlock = _mapper.Map<ReadingBlock>(createDto);
            await _readingBlockCommandService.Add(readingBlock);
        }
    }
}
