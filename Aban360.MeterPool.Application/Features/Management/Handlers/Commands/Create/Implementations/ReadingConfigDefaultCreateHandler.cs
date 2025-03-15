using Aban360.Common.Extensions;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Create.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Features.Management.Commands.Contracts;
using AutoMapper;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Create.Implementations
{
    internal sealed class ReadingConfigDefaultCreateHandler : IReadingConfigDefaultCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IReadingConfigDefaultCommandService _readingConfigDefaultCommandService;
        private readonly IHeadquartersAddhoc _headquarterAddhoc;
        public ReadingConfigDefaultCreateHandler(
            IMapper mapper,
            IReadingConfigDefaultCommandService readingConfigDefaultCommandService,
            IHeadquartersAddhoc headquarterAddhoc)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _readingConfigDefaultCommandService = readingConfigDefaultCommandService;
            _readingConfigDefaultCommandService.NotNull(nameof(_readingConfigDefaultCommandService));

            _headquarterAddhoc = headquarterAddhoc;
            _headquarterAddhoc.NotNull(nameof(_headquarterAddhoc));
        }

        public async Task Handle(ReadingConfigDefaultCreateDto createDto, CancellationToken cancellationToken)
        {
            ReadingConfigDefault readingConfigDefault = _mapper.Map<ReadingConfigDefault>(createDto);
            var headquarterTitle = await _headquarterAddhoc.Handle(createDto.HeadquartersId, cancellationToken);
            readingConfigDefault.HeadquartersTitle = headquarterTitle;

            await _readingConfigDefaultCommandService.Add(readingConfigDefault);
        }
    }
}
