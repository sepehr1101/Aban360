using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations;
using Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Base;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Implementations
{
    internal sealed class AmountConfirmedDetailHandler : IAmountConfirmedDetailHandler
    {
        private readonly ICommonZoneService _zoneService;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly IKartQueryService _kartQueryService;
        private readonly IGhestQueryService _ghestQueryService;
        private static int _currentStatusCode = (int)RequestStatusEnum.AmountIsConfirmed;
        public AmountConfirmedDetailHandler(
            ICommonZoneService zoneService,
            ITrackingQueryService trackingQueryService,
            IMoshtrakQueryService moshtrakQueryService,
            IKartQueryService kartQueryService,
            IGhestQueryService ghestQueryService)
        {
            _zoneService = zoneService;
            _zoneService.NotNull(nameof(zoneService));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _kartQueryService = kartQueryService;
            _kartQueryService.NotNull(nameof(kartQueryService));

            _ghestQueryService = ghestQueryService;
            _ghestQueryService.NotNull(nameof(ghestQueryService));
        }

        public async Task<AmountConfirmedOutputDto> Handle(Guid id, IAppUser appUser, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.Get(id);
            await _zoneService.IsUserInZone(appUser, trackingInfo.ZoneId);
            if (trackingInfo.StatusId != _currentStatusCode)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
            }

            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, trackingInfo.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber, true)).FirstOrDefault();
            MoshtrakServiceDto sData = MoshtrakService.GetMoshtrakServiceDto(moshtrakInfo);
            IEnumerable<NumericDictionary> s = MoshtrakService.GetServicesSelectedDto(sData, trackingInfo.ServiceGroupId);
           
            IEnumerable<InstallmentRequestDataOutputDto> installmentsInfo = await _ghestQueryService.Get(trackingInfo.TrackNumber.ToString(), trackingInfo.ZoneId);
            IEnumerable<OfferingAmountOutputDto> offeringsInfo = await _kartQueryService.GetByTrackNumber(ReportLiterals.Karten75, trackingInfo.TrackNumber.ToString(), trackingInfo.ZoneId);
            if (!offeringsInfo.Any())
            {
                offeringsInfo = await _kartQueryService.GetByTrackNumber(ReportLiterals.Kart, trackingInfo.TrackNumber.ToString(), trackingInfo.ZoneId);
            }

            AmountConfirmedOutputDto result = GetOutput(trackingInfo, offeringsInfo, installmentsInfo);
            return result;
        }
        private AmountConfirmedOutputDto GetOutput(TrackingOutputDto trackingInfo, IEnumerable<OfferingAmountOutputDto> offerings, IEnumerable<InstallmentRequestDataOutputDto> installmentsInfo, IEnumerable<NumericDictionary> s)
        {
            long sumAmount = offerings?.Sum(x => x.Amount) ?? 0;
            long sumDiscount = offerings?.Sum(x => x.Discount) ?? 0;
            return new AmountConfirmedOutputDto()
            {
                Offerings = offerings,
                OfferingAmount = sumAmount,
                OfferingDiscount = sumDiscount,
                OfferingPayable = sumAmount - sumDiscount,
                IstallmentsAndPayments = installmentsInfo,
                IstallmentAndPaymentAmount = installmentsInfo?.Sum(x => x.Amount) ?? 0,

                BillId = trackingInfo.BillId ?? string.Empty,
                ZoneId = trackingInfo.ZoneId,
                ZoneTitle = trackingInfo.ZoneTitle,
                RegionId = trackingInfo.RegionId,
                RegionTitle = trackingInfo.RegionTitle,
                ServiceSelected = s
            };
        }
    }
}
