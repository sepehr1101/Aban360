using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class InstallmentDisplayHandler : IInstallmentDisplayHandler
    {
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IGhestQueryService _ghestQueryService;
        static string _title = "تقسیط";
        public InstallmentDisplayHandler(
            IMoshtrakQueryService moshtrakQueryService,
            ITrackingQueryService trackingQueryService,
            IGhestQueryService ghestQueryService)
        {
            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _ghestQueryService = ghestQueryService;
            _ghestQueryService.NotNull(nameof(ghestQueryService));
        }

        public async Task<ReportOutput<InstallmentRequestHeaderOutputDto, InstallmentRequestDataOutputDto>> Handle(int trackNumber, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(trackNumber);
            MoshtrakOutputDto? moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, trackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();

            IEnumerable<InstallmentRequestDataOutputDto> karts = await _ghestQueryService.Get(trackingInfo.StringTrackNumber, trackingInfo.ZoneId);

            InstallmentRequestHeaderOutputDto header = new()
            {
                Amount = karts?.Sum(x => x.Amount) ?? 0,
                InstallmentCount = karts?.Count() ?? 0,
                PrePaymentAmount = karts?.FirstOrDefault()?.Amount ?? 0,
                PrePaymentPercent = 0,//todo,
                PerPaymentAmount = (karts?.Count() ?? 0) > 1 ? karts?.ElementAt(1)?.Amount ?? 0 : 0,//todo
                TrackNumber = trackNumber,
                ServiceGroupTitle = trackingInfo.ServiceGroupTitle,
                BillId = trackingInfo.BillId,
                NeighbourBillId = moshtrakInfo?.NeighbourBillId ?? string.Empty,
                RegionTitle = trackingInfo.RegionTitle,
                ZoneTitle = trackingInfo.ZoneTitle,
                FullName = $"{moshtrakInfo?.FirstName ?? string.Empty} {moshtrakInfo?.Surname ?? string.Empty}",
                RecordCount = karts?.Count() ?? 0,
                Title = _title,
            };
            ReportOutput<InstallmentRequestHeaderOutputDto, InstallmentRequestDataOutputDto> result = new(_title, header, karts);

            return result;
        }
    }
}
