using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class InstallmentDisplayHandler : IInstallmentDisplayHandler
    {
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IGhestQueryService _ghestQueryService;
        private readonly IKartQueryService _kartQueryService;
        static string _title = "تقسیط";
        static long _MinAmount = 10_000_000;
        public InstallmentDisplayHandler(
            IMoshtrakQueryService moshtrakQueryService,
            ITrackingQueryService trackingQueryService,
            IGhestQueryService ghestQueryService,
            IKartQueryService kartQueryService)
        {
            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _ghestQueryService = ghestQueryService;
            _ghestQueryService.NotNull(nameof(ghestQueryService));

            _kartQueryService = kartQueryService;
            _kartQueryService.NotNull(nameof(kartQueryService));
        }

        public async Task<ReportOutput<InstallmentRequestHeaderOutputDto, InstallmentRequestDataOutputDto>> Handle(int trackNumber, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(trackNumber);
            MoshtrakOutputDto? moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, trackNumber), MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();

            IEnumerable<KartGetDto> karts = await _kartQueryService.GetAll(trackingInfo.StringTrackNumber, trackingInfo.ZoneId);
            if (karts?.Count() == 0 || (karts?.FirstOrDefault()?.FinalAmount ?? 0) < _MinAmount)
            {
                throw new InvalidTrackingException(ExceptionLiterals.NotCalculation);
            }
            IEnumerable<InstallmentRequestDataOutputDto> ghests = await _ghestQueryService.Get(trackingInfo.StringTrackNumber, trackingInfo.ZoneId);
            int installmentCount = ghests?.Count() ?? 0;

            InstallmentRequestHeaderOutputDto header = new()
            {
                Amount = ghests?.Sum(x => x.Amount) ?? 0,
                InstallmentCount = karts?.FirstOrDefault()?.InstallmentCount ?? 0,
                PrePaymentAmount = karts?.FirstOrDefault()?.FirstInstallment ?? 0,
                PrePaymentPercent = karts?.FirstOrDefault()?.InstallmentPercent ?? 0,
                InstallmentAmount = karts?.FirstOrDefault()?.Installment ?? 0,
                TrackNumber = trackNumber,
                ServiceGroupTitle = trackingInfo.ServiceGroupTitle,
                BillId = trackingInfo.BillId,
                RegionTitle = trackingInfo.RegionTitle,
                ZoneTitle = trackingInfo.ZoneTitle,
                FullName = $"{moshtrakInfo?.FirstName ?? string.Empty} {moshtrakInfo?.Surname ?? string.Empty}",
                RecordCount = ghests?.Count() ?? 0,
                Title = _title,
            };
            ReportOutput<InstallmentRequestHeaderOutputDto, InstallmentRequestDataOutputDto> result = new(_title, header, ghests);

            return result;
        }
    }
}
