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
    internal sealed class CalculationConfirmedDetailHandler : ICalculationConfirmedDetailHandler
    {
        private readonly ICommonZoneService _zoneService;
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly ITrackingQueryService _trackingQueryService;
        private static int[] _currentStatusCodes = { (int)RequestStatusEnum.CalculationConfirmd, (int)RequestStatusEnum.SkipSpecifyRadif };
        public CalculationConfirmedDetailHandler(
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

        public async Task<CalculationConfirmedOutputDto> Handle(Guid id, IAppUser appUser, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.Get(id);
            await _zoneService.IsUserInZone(appUser, trackingInfo.ZoneId);
            if (!_currentStatusCodes.Contains(trackingInfo.StatusId))
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidStatusId);
            }

            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, trackingInfo.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber, true)).FirstOrDefault();
            MoshtrakServiceDto sData = MoshtrakService.GetMoshtrakServiceDto(moshtrakInfo);
            IEnumerable<NumericDictionary> s = MoshtrakService.GetServicesSelectedDto(sData, trackingInfo.ServiceGroupId);
            CalculationConfirmedOutputDto result = GetOutput(trackingInfo, moshtrakInfo, s);

            return result;
        }
        private CalculationConfirmedOutputDto GetOutput(TrackingOutputDto trackingInfo, MoshtrakOutputDto moshtrakInfo, IEnumerable<NumericDictionary> s)
        {
            return new CalculationConfirmedOutputDto()
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
                NotificationNumber = moshtrakInfo.NotificationMobile ?? string.Empty,
                Address = moshtrakInfo.Address,
                UsageId = moshtrakInfo.UsageId,
                UsageTitle = moshtrakInfo.UsageTitle,
                Siphon100 = moshtrakInfo.Siphon100,
                Siphon125 = moshtrakInfo.Siphon125,
                Siphon150 = moshtrakInfo.Siphon150,
                Siphon200 = moshtrakInfo.Siphon200,
                Premises = moshtrakInfo.Premises,
                ImprovementOverall = moshtrakInfo.ImprovementOverall,
                ImprovementDomestic = moshtrakInfo.ImprovementDomestic,
                ImprovementCommertial = moshtrakInfo.ImprovementCommercial,
                CommertialUnit = moshtrakInfo.CommercialUnit,
                DomesticUnit = moshtrakInfo.DomesticUnit,
                OtherUnit = moshtrakInfo.OtherUnit,
                FamilyCount = moshtrakInfo.FamilyCount,
                HouseholdNumber = moshtrakInfo.HouseholdNumber,
                DiscountTypeId = moshtrakInfo.DiscountTypeId,
                DiscountTypeTitle = moshtrakInfo.DiscountTypeTitle,
                DiscountCount = moshtrakInfo.DiscountCount,
                HasBroker = moshtrakInfo.HasBroker,
                ContractualCapacity = moshtrakInfo.ContractualCapacity,
                BranchTypeId = moshtrakInfo.BranchTypeId,
                BranchTypeTitle = moshtrakInfo.BranchTypeTitle,
                RegionMultiplier = moshtrakInfo.RegionMultiplier,
                MeterTypeId = moshtrakInfo.CounterType,
                MeterTypeTitle = moshtrakInfo.CounterTypeTitle ?? string.Empty,
                MeterDiamterId = moshtrakInfo.MeterDiameterId,
                MeterDiamterTitle = moshtrakInfo.MeterDiameterTitle,
                PostalCode = moshtrakInfo.PostalCode ?? string.Empty,
                Description = trackingInfo.Description,
                CompanyServiceSelected = s.ToList(),

            };
        }
    }
}
