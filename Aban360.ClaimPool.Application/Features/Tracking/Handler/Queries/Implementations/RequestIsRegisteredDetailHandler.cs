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
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Tracking.Handler.Queries.Implementations
{
    internal sealed class RequestIsRegisteredDetailHandler : IRequestIsRegisteredDetailHandler
    {
        private readonly ICommonZoneService _zoneService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly ITrackingQueryService _trackingQueryService;
        private static int _currentStatusCode = (int)RequestStatusEnum.RequestIsRegisterd;
        public RequestIsRegisteredDetailHandler(
            ICommonZoneService zoneService,
            IMoshtrakQueryService moshtrakQueryService,
            ITrackingQueryService trackingQueryService)
        {
            _zoneService = zoneService;
            _zoneService.NotNull(nameof(zoneService));

            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));
        }

        public async Task<RequestIsRegisterdOutputDto> Handle(Guid id, IAppUser appUser, CancellationToken cancellationToken)
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

            RequestIsRegisterdOutputDto result = GetOutput(trackingInfo, moshtrakInfo, s);
            return result;
        }
        private RequestIsRegisterdOutputDto GetOutput(TrackingOutputDto trackingInfo, MoshtrakOutputDto moshtrakInfo, IEnumerable<NumericDictionary> s)
        {
            return new RequestIsRegisterdOutputDto()
            {
                TrackNumber = trackingInfo.TrackNumber,
                BillId = trackingInfo.BillId ?? string.Empty,
                NeighbourBillId = trackingInfo.NeighbourBillId,
                ZoneId = trackingInfo.ZoneId,
                ZoneTitle = trackingInfo.ZoneTitle,
                RegionId = trackingInfo.RegionId,
                RegionTitle = trackingInfo.RegionTitle,
                FirstName = moshtrakInfo.FirstName ?? string.Empty,
                Surname = moshtrakInfo.Surname ?? string.Empty,
                FatherName = moshtrakInfo.FatherName,
                NationalCode = moshtrakInfo.NationalCode,
                MobileNumber = moshtrakInfo.MobileNumber,
                PhoneNumber = moshtrakInfo.PhoneNumber,
                Caller = trackingInfo.Caller,
                NotificationNumber = moshtrakInfo.NotificationMobile ?? string.Empty,
                Address = moshtrakInfo.Address,
                CompanyServiceSelected = s.ToList()
            };
        }
    }
}
