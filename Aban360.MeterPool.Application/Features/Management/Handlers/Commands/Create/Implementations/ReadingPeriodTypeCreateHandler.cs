using Aban360.Common.Extensions;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Create.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Features.Manegement.Commands.Contracts;
using AutoMapper;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Create.Implementations
{
    internal sealed class ReadingPeriodTypeCreateHandler : IReadingPeriodTypeCreateHandler
    {
        private readonly IMapper _mapper;
        private readonly IReadingPeriodTypeCommandService _readingPeriodTypeCommandService;
        private readonly IHeadquartersAddhoc _headquarterAddhoc;
        public ReadingPeriodTypeCreateHandler(
            IMapper mapper,
            IReadingPeriodTypeCommandService readingPeriodTypeCommandService,
            IHeadquartersAddhoc headquarterAddhoc)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _readingPeriodTypeCommandService = readingPeriodTypeCommandService;
            _readingPeriodTypeCommandService.NotNull(nameof(_readingPeriodTypeCommandService));

            _headquarterAddhoc = headquarterAddhoc;
            _headquarterAddhoc.NotNull(nameof(_headquarterAddhoc));
        }

        public async Task Handle(ReadingPeriodTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            var readingPeriodTypeDto = _mapper.Map<ReadingPeriodType>(createDto);
            if (readingPeriodTypeDto == null)
            {
                throw new InvalidDataException();
            }
            var headquarterTitle=await _headquarterAddhoc.Handle(createDto.HeadquartersId,cancellationToken);
            if (headquarterTitle == null)
            {
                throw new InvalidDataException();
            }

            var readingPeriodType = _mapper.Map<ReadingPeriodType>(createDto);
            readingPeriodType.HeadquartersTitle = headquarterTitle;

            await _readingPeriodTypeCommandService.Add(readingPeriodType);
        }
    }
}
