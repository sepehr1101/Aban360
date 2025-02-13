using Aban360.Common.Extensions;
using Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Update.Contracts;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using Aban360.LocationPool.Persistence.Features.MainHirearchy.Queries.Contracts;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Update.Implementations
{
    public class ReadingBoundUpdateHandler : IReadingBoundUpdateHandler
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
            var readingBound = await _readingBoundQueryService.Get(updateDto.Id);
            if (readingBound == null)
            {
                throw new InvalidDataException();
            }
            _mapper.Map(updateDto, readingBound);
        }
    }
}
