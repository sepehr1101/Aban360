using Aban360.Common.Extensions;
using Aban360.LocationPool.GatewayAdhoc.Features.MainHirearchy.Contracts;
using Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Update.Contracts;
using Aban360.MeterPool.Domain.Features.Management.Dtos.Commands;
using Aban360.MeterPool.Domain.Features.Management.Entities;
using Aban360.MeterPool.Persistence.Features.Manegement.Queries.Contracts;
using AutoMapper;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Commands.Update.Implementations
{
    internal sealed class ReadingPeriodTypeUpdateHandler : IReadingPeriodTypeUpdateHandler
    {
        private readonly IMapper _mapper;
        private readonly IReadingPeriodTypeQueryService _readingPeriodTypeQueryService;
        private readonly IHeadquartersAddhoc _headquarterAddhoc;
        public ReadingPeriodTypeUpdateHandler(
            IMapper mapper,
            IReadingPeriodTypeQueryService readingPeriodTypeQueryService,
            IHeadquartersAddhoc headquarterAddhoc)
        {
            _mapper = mapper;
            _mapper.NotNull(nameof(_mapper));

            _readingPeriodTypeQueryService = readingPeriodTypeQueryService;
            _readingPeriodTypeQueryService.NotNull(nameof(_readingPeriodTypeQueryService));

            _headquarterAddhoc = headquarterAddhoc;
            _headquarterAddhoc.NotNull(nameof(_headquarterAddhoc));
        }

        public async Task Handle(ReadingPeriodTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            ReadingPeriodType readingPeriodType = await _readingPeriodTypeQueryService.Get(updateDto.Id);
            _mapper.Map(updateDto, readingPeriodType);
            string headquarterTitle = await _headquarterAddhoc.Handle(updateDto.HeadquartersId, cancellationToken);
            readingPeriodType.HeadquartersTitle = headquarterTitle;
        }
    }
}