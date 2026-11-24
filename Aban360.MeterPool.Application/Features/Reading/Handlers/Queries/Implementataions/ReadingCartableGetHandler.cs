using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.MeterPool.Application.Features.Reading.Handlers.Queries.Contracts;
using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;
using Aban360.MeterPool.Persistence.Features.Reading.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;

namespace Aban360.MeterPool.Application.Features.Reading.Handlers.Queries.Implementataions
{
    internal sealed class ReadingCartableGetHandler : IReadingCartableGetHandler
    {
        private readonly ICommonZoneService _zoneService;
        private readonly ITrackingQueryService _trackingQueryService;
        private static string _title = ReportLiterals.ReadingCartable;
        public ReadingCartableGetHandler(ICommonZoneService zoneService, ITrackingQueryService trackingQueryService)
        {
            _zoneService = zoneService;
            _zoneService.NotNull(nameof(zoneService));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));
        }

        public async Task<ReportOutput<MeterReadingCartableHeaderOutputDto, MeterReadingCartableDataOutputDto>> Handle(MeterReadingCartableInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            IEnumerable<int> myZoneIds = await _zoneService.GetMyZoneIds(appUser);
            MeterReadingCartableGetDto cartableInputDto = new(inputDto.FromReadingNumber, inputDto.ToReadingNumber, myZoneIds);
            IEnumerable<MeterReadingCartableDataOutputDto> data = await _trackingQueryService.Get(cartableInputDto);
            MeterReadingCartableHeaderOutputDto header = new()
            {
                FromReadingNumber = inputDto.FromReadingNumber,
                ToReadingNumber = inputDto.ToReadingNumber,
                RecordCount = data?.Count() ?? 0,
                CustomerCount = data?.Sum(d => d.OverallCustomerCount) ?? 0,
                Title = _title,
            };

            ReportOutput<MeterReadingCartableHeaderOutputDto, MeterReadingCartableDataOutputDto> result = new(_title, header, data);
            return result;
        }
    }
}
