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
        public ToCalulationConfirmHandler(
            IMoshtrakQueryService moshtrakQueryService,
            ITrackingQueryService trackingQueryService,
            IExaminationQueryService assessmentQueryService,
            ICommonZoneService commonZoneService)
        {
            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));

            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));

            _assessmentQueryService = assessmentQueryService;
            _assessmentQueryService.NotNull(nameof(assessmentQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));
        }

        public async Task<ToCalculationConfirmGetDto> Handle(Guid trackId, IAppUser appUser, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.Get(trackId);
            MoshtrakGetDto moshtrackSearch = new(trackingInfo.ZoneId, null, null, trackingInfo.TrackNumber);
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(moshtrackSearch, MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            AssessmentDataOutputDto assessmentInfo = await _assessmentQueryService.GetByTrackId(trackId);
            await _commonZoneService.IsUserInZone(appUser, trackingInfo.ZoneId);

            MoshtrakServiceDto sData = GetSDto(moshtrakInfo);
            IEnumerable<SelectionDto> companyServices = MoshtrakService.GetMoshtrakCompanyServiceDto(sData, trackingInfo.ServiceGroupId);

            ToCalculationConfirmGetDto moshtrakData = GetData(moshtrakInfo, companyServices, trackingInfo, assessmentInfo);
            return moshtrakData;
        }
        private MoshtrakServiceDto GetSDto(MoshtrakOutputDto input)
        {
            return new MoshtrakServiceDto()
            {
                s0 = input.s0,
                s1 = input.s1,
                s2 = input.s2,
                s3 = input.s3,
                s4 = input.s4,
                s5 = input.s5,
                s8 = input.s8,
                s9 = input.s9,
                s10 = input.s10,
                s11 = input.s11,
                s12 = input.s12,
                s13 = input.s13,
                s14 = input.s14,
                s15 = input.s15,
                s16 = input.s16,
                s17 = input.s17,
                s18 = input.s18,
                s19 = input.s19,
                s20 = input.s20,
                s21 = input.s21,
                s22 = input.s22,
                s23 = input.s23,
                s24 = input.s24,
                s25 = input.s25,
                s26 = input.s26,
                s27 = input.s27,
                s28 = input.s28,
                s29 = input.s29,
                s30 = input.s30,
                s31 = input.s31,
                s32 = input.s32,
                s33 = input.s33,
                s34 = input.s34,
                s35 = input.s35,
                s36 = input.s36,
                s37 = input.s37,
                s38 = input.s38,
                s39 = input.s39,
                s40 = input.s40,
                s41 = input.s41,
                s42 = input.s42,
                s43 = input.s43,
                s44 = input.s44,
                s45 = input.s45,
                s46 = input.s46,
                s47 = input.s47,
                s48 = input.s48,
            };
        }
        private ToCalculationConfirmGetDto GetData(MoshtrakOutputDto moshtrakInfo, IEnumerable<SelectionDto> companyServices, TrackingOutputDto trackingInfo, AssessmentDataOutputDto assessmentInfo)
        {
            return new ToCalculationConfirmGetDto()
            {
                ZoneId = moshtrakInfo.ZoneId,
                ZoneTitle = moshtrakInfo.ZoneTitle,
                CustomerNumber = moshtrakInfo.CustomerNumber,
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
