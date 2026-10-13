using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using FluentValidation;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    public interface IToCalulationConfirmHandler
    {
        Task<ToCalculationConfirmGetDto> Handle(Guid trackId, IAppUser appUser, CancellationToken cancellationToken);
    }
    internal sealed class ToCalulationConfirmHandler : IToCalulationConfirmHandler
    {
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IExaminationQueryService _assessmentQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IMoshtrakService _moshtrakService;
        public ToCalulationConfirmHandler(
            IMoshtrakQueryService moshtrakQueryService,
            ITrackingQueryService trackingQueryService,
            IExaminationQueryService assessmentQueryService,
            ICommonZoneService commonZoneService,
            IMoshtrakService moshtrakService)
        {
            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _assessmentQueryService = assessmentQueryService;
            _assessmentQueryService.NotNull(nameof(assessmentQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));

            _moshtrakService = moshtrakService;
            _moshtrakService.NotNull(nameof(moshtrakService));
        }

        public async Task<ToCalculationConfirmGetDto> Handle(Guid trackId, IAppUser appUser, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.Get(trackId);
            MoshtrakGetDto moshtrackSearch = new(trackingInfo.ZoneId, null, null, trackingInfo.TrackNumber);
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(moshtrackSearch, MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            AssessmentDataOutputDto assessmentInfo = await _assessmentQueryService.GetByTrackId(trackId, true);
            await _commonZoneService.IsUserInZone(appUser, trackingInfo.ZoneId);

            MoshtrakServiceDto sData = _moshtrakService.GetMoshtrakServiceDto(moshtrakInfo);
            IEnumerable<SelectionDto> companyServices = await _moshtrakService.GetMoshtrakCompanyServiceDto(sData, trackingInfo.ServiceGroupId);

            ToCalculationConfirmGetDto moshtrakData = GetData(moshtrakInfo, companyServices, trackingInfo, assessmentInfo);
            return moshtrakData;
        }
        private ToCalculationConfirmGetDto GetData(MoshtrakOutputDto moshtrakInfo, IEnumerable<SelectionDto> companyServices, TrackingOutputDto trackingInfo, AssessmentDataOutputDto assessmentInfo)
        {
            return new ToCalculationConfirmGetDto()
            {
                ZoneId = moshtrakInfo.ZoneId,
                ZoneTitle = moshtrakInfo.ZoneTitle,
                CustomerNumber = moshtrakInfo.CustomerNumber,
                BillId = trackingInfo.BillId ?? string.Empty,
                ReadingNumber = moshtrakInfo.ReadingNumber,
                FirstName = moshtrakInfo.FirstName,
                Surname = moshtrakInfo.Surname,
                FatherName = moshtrakInfo.FatherName,
                NationalCode = moshtrakInfo.NationalCode,
                PhoneNumber = moshtrakInfo.PhoneNumber,
                MobileNumber = moshtrakInfo.MobileNumber,
                RequestDateJalali = moshtrakInfo.RequestDateJalali,
                Address = moshtrakInfo.Address,
                PostalCode = moshtrakInfo.PostalCode,
                NeighbourBillId = moshtrakInfo.NeighbourBillId,
                TrackNumber = moshtrakInfo.TrackNumber,
                UsageId = moshtrakInfo.UsageId,
                UsageTitle = moshtrakInfo.UsageTitle,
                IsRegistered = moshtrakInfo.IsRegistered,
                BranchTypeId = moshtrakInfo.BranchTypeId,
                BranchTypeTitle = moshtrakInfo.BranchTypeTitle,
                Premises = moshtrakInfo.Premises,
                ImprovementOverall = moshtrakInfo.ImprovementOverall,
                ImprovementDomestic = moshtrakInfo.ImprovementDomestic,
                ImprovementCommercial = moshtrakInfo.ImprovementCommercial,
                OtherUnit = moshtrakInfo.OtherUnit,
                DomesticUnit = moshtrakInfo.DomesticUnit,
                CommercialUnit = moshtrakInfo.CommercialUnit,
                TotalUnit = moshtrakInfo.OtherUnit + moshtrakInfo.DomesticUnit + moshtrakInfo.CommercialUnit,
                ContractualCapacity = moshtrakInfo.ContractualCapacity,
                Siphon100 = moshtrakInfo.Siphon100,
                Siphon125 = moshtrakInfo.Siphon125,
                Siphon150 = moshtrakInfo.Siphon150,
                Siphon200 = moshtrakInfo.Siphon200,
                MainSiphon = moshtrakInfo.MainSiphon,
                CommonSiphon = moshtrakInfo.CommonSiphon,
                MeterDiameterTitle = moshtrakInfo.MeterDiameterTitle,
                MeterDiameterId = moshtrakInfo.MeterDiameterId,
                DiscountTypeId = moshtrakInfo.DiscountTypeId,
                DiscountTypeTitle = moshtrakInfo.DiscountTypeTitle,
                DiscountCount = moshtrakInfo.DiscountCount,
                IsSpecial = moshtrakInfo.IsSpecial,
                CounterType = moshtrakInfo.CounterType,
                NotificationNumber = moshtrakInfo.NotificationMobile,
                Description = moshtrakInfo.Description,
                HouseValue = moshtrakInfo.HouseValue,
                IsNonPermanent = moshtrakInfo.IsNonPermanent,
                BlockId = moshtrakInfo.BlockId,
                BrokerId = moshtrakInfo.BrokerId,
                ServiceGroupTitle = trackingInfo.ServiceGroupTitle,
                ServiceGroupId = trackingInfo.ServiceGroupId,
                AssessmentCode = assessmentInfo.AssessmentCode,
                AssessmentDateJalali = assessmentInfo.AssessmentDateJalali,
                AssessmentMobile = assessmentInfo.AssessmentMobile,
                AssessmentName = assessmentInfo.AssessmentName,
                CompanyServiceItems = companyServices
            };
        }
    }
}
