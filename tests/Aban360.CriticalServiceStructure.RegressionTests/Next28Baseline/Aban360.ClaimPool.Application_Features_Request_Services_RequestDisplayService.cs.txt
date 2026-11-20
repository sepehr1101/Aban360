using Aban360.ClaimPool.Application.Features.Request.Handler.Commands.Create.Implementations;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using FluentValidation;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Services;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using Aban360.UserPool.Persistence.Constants.Enums;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using DNTPersianUtils.Core;
using Aban360.ClaimPool.Application.Features.Request.Services;
using Aban360.ReportPool.Domain.Base;
using Aban360.UserPool.Application.Features.Auth.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Request.Services
{
    public interface IRequestDisplayService
    {
        Task<MoshtrakDataOutputDto> DisplayRequestAsync(int trackNumber, CancellationToken cancellationToken);
        Task<MoshtrakDataOutputDto> DisplayRequestByTrackIdAsync(Guid trackId, IAppUser appUser, CancellationToken cancellationToken);
        Task<ReportOutput<TrackingKartableHeaderOutputDto, TrackingKartableDataOutputDto>> KartableRequestGetAllAsync(IAppUser currentUser, CancellationToken cancellationToken);
        Task<RequestBasicInfoDataOutputDto> RequestBasicInfoGetAsync(int trackNumber, IAppUser appUser, CancellationToken cancellationToken);
        Task<IEnumerable<PreviousRequestDataOutputDto>> PreviousRequestGetByBillIdAsync(string billId, CancellationToken cancellationToken);
        ICollection<NumericDictionary> KitCodeDictionaryGetAsync(CancellationToken cancellationToken);
        IEnumerable<StringDictionary> ReadingBlockListGetAsync(CancellationToken cancellationToken);
        Task<ToCalculationConfirmGetDto> ToCalulationConfirmAsync(Guid trackId, IAppUser appUser, CancellationToken cancellationToken);
        Task<ToSetAssessmentTimeGetDto> ToSetAssessmentTimeGetByTrackIdAsync(Guid trackId, IAppUser appUser, CancellationToken cancellationToken);
        Task<ToSetReAssessmentTimeGetDto> ToSetReAssessmentTimeGetByTrackIdAsync(Guid trackId, IAppUser appUser, CancellationToken cancellationToken);
    }

    internal sealed class RequestDisplayService : IRequestDisplayService
    {
        private readonly IMoshtrakQueryService _moshtrakQueryService;
        private readonly ITrackingQueryService _trackingQueryService;
        private readonly IMoshtrakService _moshtrakService;
        private readonly IValidator<ZoneIdAndTrackNumber> _validator;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IUserClaimQueryService _userClaimQueryService;
        static string _KartableRequestGetAlltitle = "کارتابل درخواست‌ها";
        private readonly ICommonMemberQueryService _commonMemberQueryService;
        private readonly IExaminationQueryService _assessmentQueryService;
        private readonly IMotherChildRequestService _motherChildRequestService;
        private static int _ToCalulationConfirmassessmentResultId = (int)RequestStatusEnum.SetExaminationResult;
        private readonly IAssessmentGetByZoneIdHandler _assessmentGetByZoneIdHandler;
        private readonly IUserRoleQueryService _userRoleQueryService;
        private static int _ToSetAssessmentTimeGetByTrackIdreaderRoleId = ReportLiterals.AssessmentRoleId;
        public RequestDisplayService(IMoshtrakQueryService moshtrakQueryService, ITrackingQueryService trackingQueryService, IMoshtrakService moshtrakService, IValidator<ZoneIdAndTrackNumber> validator, ICommonZoneService commonZoneService, IUserClaimQueryService userClaimQueryService, ICommonMemberQueryService commonMemberQueryService, IExaminationQueryService assessmentQueryService, IMotherChildRequestService motherChildRequestService, IUserRoleQueryService userRoleQueryService, IAssessmentGetByZoneIdHandler assessmentGetByZoneIdHandler)
        {
            _moshtrakQueryService = moshtrakQueryService;
            _moshtrakQueryService.NotNull(nameof(moshtrakQueryService));
            _trackingQueryService = trackingQueryService;
            _trackingQueryService.NotNull(nameof(trackingQueryService));
            _moshtrakService = moshtrakService;
            _moshtrakService.NotNull(nameof(moshtrakService));
            _validator = validator;
            _validator.NotNull(nameof(validator));
            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));
            _userClaimQueryService = userClaimQueryService;
            _userClaimQueryService.NotNull(nameof(userClaimQueryService));
            _commonMemberQueryService = commonMemberQueryService;
            _commonMemberQueryService.NotNull(nameof(commonMemberQueryService));
            _assessmentQueryService = assessmentQueryService;
            _assessmentQueryService.NotNull(nameof(assessmentQueryService));
            _motherChildRequestService = motherChildRequestService;
            _motherChildRequestService.NotNull(nameof(motherChildRequestService));
            _userRoleQueryService = userRoleQueryService;
            _userRoleQueryService.NotNull(nameof(userRoleQueryService));
            _assessmentGetByZoneIdHandler = assessmentGetByZoneIdHandler;
            _assessmentGetByZoneIdHandler.NotNull(nameof(assessmentGetByZoneIdHandler));
        }

        public async Task<MoshtrakDataOutputDto> DisplayRequestAsync(int trackNumber, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.GetLatest(trackNumber);
            MoshtrakGetDto moshtrackSearch = new(trackingInfo.ZoneId, null, null, trackNumber);
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(moshtrackSearch, MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            MoshtrakServiceDto sData = DisplayRequestGetSDto(moshtrakInfo);
            IEnumerable<SelectionDto> companyServices = await _moshtrakService.GetMoshtrakCompanyServiceDto(sData, trackingInfo.ServiceGroupId);
            MoshtrakDataOutputDto moshtrakData = DisplayRequestGetMoshtrakData(moshtrakInfo, companyServices, trackingInfo);
            return moshtrakData;
        }

        private MoshtrakServiceDto DisplayRequestGetSDto(MoshtrakOutputDto input)
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

        private MoshtrakDataOutputDto DisplayRequestGetMoshtrakData(MoshtrakOutputDto input, IEnumerable<SelectionDto> companyServices, TrackingOutputDto trackingInfo)
        {
            return new MoshtrakDataOutputDto()
            {
                Id = input.Id,
                ZoneId = input.ZoneId,
                ZoneTitle = input.ZoneTitle,
                CustomerNumber = input.CustomerNumber,
                ReadingNumber = input.ReadingNumber,
                FirstName = input.FirstName,
                Surname = input.Surname,
                FatherName = input.FatherName,
                NationalCode = input.NationalCode,
                PhoneNumber = input.PhoneNumber,
                MobileNumber = input.MobileNumber,
                RequestDateJalali = input.RequestDateJalali,
                Address = input.Address,
                PostalCode = input.PostalCode,
                NeighbourBillId = input.NeighbourBillId,
                BillId = trackingInfo.BillId ?? string.Empty,
                TrackNumber = input.TrackNumber,
                UsageId = input.UsageId,
                UsageTitle = input.UsageTitle,
                IsRegistered = input.IsRegistered,
                BranchTypeId = input.BranchTypeId,
                BranchTypeTitle = input.BranchTypeTitle,
                Premises = input.Premises,
                ImprovementOverall = input.ImprovementOverall,
                ImprovementDomestic = input.ImprovementDomestic,
                ImprovementCommercial = input.ImprovementCommercial,
                OtherUnit = input.OtherUnit,
                DomesticUnit = input.DomesticUnit,
                CommercialUnit = input.CommercialUnit,
                ContractualCapacity = input.ContractualCapacity,
                Siphon100 = input.Siphon100,
                Siphon125 = input.Siphon125,
                Siphon150 = input.Siphon150,
                Siphon200 = input.Siphon200,
                MainSiphon = input.MainSiphon,
                CommonSiphon = input.CommonSiphon,
                MeterDiameterTitle = input.MeterDiameterTitle,
                MeterDiameterId = input.MeterDiameterId,
                DiscountTypeId = input.DiscountTypeId,
                DiscountTypeTitle = input.DiscountTypeTitle,
                DiscountCount = input.DiscountCount,
                IsSpecial = input.IsSpecial,
                CounterType = input.CounterType,
                NotificationNumber = input.NotificationMobile,
                Description = input.Description,
                HouseValue = input.HouseValue,
                IsNonPermanent = input.IsNonPermanent,
                BlockId = input.BlockId,
                BrokerId = input.BrokerId,
                ServiceGroupTitle = trackingInfo.ServiceGroupTitle,
                ServiceGroupId = trackingInfo.ServiceGroupId,
                CompanyServiceItems = companyServices
            };
        }

        public async Task<MoshtrakDataOutputDto> DisplayRequestByTrackIdAsync(Guid trackId, IAppUser appUser, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.Get(trackId);
            MoshtrakGetDto moshtrackSearch = new(trackingInfo.ZoneId, null, null, trackingInfo.TrackNumber);
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(moshtrackSearch, MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            await _commonZoneService.IsUserInZone(appUser, trackingInfo.ZoneId);
            MoshtrakServiceDto sData = DisplayRequestByTrackIdGetSDto(moshtrakInfo);
            IEnumerable<SelectionDto> companyServices = await _moshtrakService.GetMoshtrakCompanyServiceDto(sData, trackingInfo.ServiceGroupId);
            MoshtrakDataOutputDto moshtrakData = DisplayRequestByTrackIdGetMoshtrakData(moshtrakInfo, companyServices, trackingInfo);
            return moshtrakData;
        }

        private MoshtrakServiceDto DisplayRequestByTrackIdGetSDto(MoshtrakOutputDto input)
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

        private MoshtrakDataOutputDto DisplayRequestByTrackIdGetMoshtrakData(MoshtrakOutputDto input, IEnumerable<SelectionDto> companyServices, TrackingOutputDto trackingInfo)
        {
            return new MoshtrakDataOutputDto()
            {
                Id = input.Id,
                ZoneId = input.ZoneId,
                ZoneTitle = input.ZoneTitle,
                CustomerNumber = input.CustomerNumber,
                ReadingNumber = input.ReadingNumber,
                FirstName = input.FirstName,
                Surname = input.Surname,
                FatherName = input.FatherName,
                NationalCode = input.NationalCode,
                PhoneNumber = input.PhoneNumber,
                MobileNumber = input.MobileNumber,
                RequestDateJalali = input.RequestDateJalali,
                Address = input.Address,
                PostalCode = input.PostalCode,
                NeighbourBillId = input.NeighbourBillId,
                BillId = trackingInfo.BillId ?? string.Empty,
                TrackNumber = input.TrackNumber,
                UsageId = input.UsageId,
                UsageTitle = input.UsageTitle,
                IsRegistered = input.IsRegistered,
                BranchTypeId = input.BranchTypeId,
                BranchTypeTitle = input.BranchTypeTitle,
                Premises = input.Premises,
                ImprovementOverall = input.ImprovementOverall,
                ImprovementDomestic = input.ImprovementDomestic,
                ImprovementCommercial = input.ImprovementCommercial,
                OtherUnit = input.OtherUnit,
                DomesticUnit = input.DomesticUnit,
                CommercialUnit = input.CommercialUnit,
                ContractualCapacity = input.ContractualCapacity,
                Siphon100 = input.Siphon100,
                Siphon125 = input.Siphon125,
                Siphon150 = input.Siphon150,
                Siphon200 = input.Siphon200,
                MainSiphon = input.MainSiphon,
                CommonSiphon = input.CommonSiphon,
                MeterDiameterTitle = input.MeterDiameterTitle,
                MeterDiameterId = input.MeterDiameterId,
                DiscountTypeId = input.DiscountTypeId,
                DiscountTypeTitle = input.DiscountTypeTitle,
                DiscountCount = input.DiscountCount,
                IsSpecial = input.IsSpecial,
                CounterType = input.CounterType,
                NotificationNumber = input.NotificationMobile,
                Description = input.Description,
                HouseValue = input.HouseValue,
                IsNonPermanent = input.IsNonPermanent,
                BlockId = input.BlockId,
                BrokerId = input.BrokerId,
                ServiceGroupTitle = trackingInfo.ServiceGroupTitle,
                ServiceGroupId = trackingInfo.ServiceGroupId,
                CompanyServiceItems = companyServices
            };
        }

        public async Task<ReportOutput<TrackingKartableHeaderOutputDto, TrackingKartableDataOutputDto>> KartableRequestGetAllAsync(IAppUser currentUser, CancellationToken cancellationToken)
        {
            IEnumerable<int> zoneIds = await _commonZoneService.GetMyZoneIds(currentUser);
            ICollection<UserClaim> userClaims = await _userClaimQueryService.Get(currentUser.UserId, ClaimType.RequestKartable);
            ICollection<int> userAccesskartables = userClaims.Where(u => u.ValidTo is null).Select(u => int.Parse(u.ClaimValue)).ToList();
            IEnumerable<TrackingKartableDataOutputDto> data = await _trackingQueryService.GetAllOpenRequest(zoneIds, userAccesskartables);
            TrackingKartableHeaderOutputDto header = new()
            {
                ZoneCount = data?.DistinctBy(d => d.ZoneId)?.Count() ?? 0,
                RequestCount = data?.Count() ?? 0,
                CurrentDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = _KartableRequestGetAlltitle,
            };
            return new ReportOutput<TrackingKartableHeaderOutputDto, TrackingKartableDataOutputDto>(_KartableRequestGetAlltitle, header, data);
        }

        public async Task<RequestBasicInfoDataOutputDto> RequestBasicInfoGetAsync(int trackNumber, IAppUser appUser, CancellationToken cancellationToken)
        {
            TrackingOutputDto latestTrackingInfo = await _trackingQueryService.GetLatest(trackNumber);
            await _commonZoneService.IsUserInZone(appUser, latestTrackingInfo.ZoneId);
            RequestBasicInfoGetDto basicInfo = await _moshtrakQueryService.GetBasicInfo(trackNumber, latestTrackingInfo.ZoneId);
            MoshtrakServiceDto moshtrakServiceDto = RequestBasicInfoGetGetMoshtrakService(basicInfo);
            IEnumerable<SelectionDto> ServiceSelected = await _moshtrakService.GetMoshtrakCompanyServiceDto(moshtrakServiceDto, latestTrackingInfo.ServiceGroupId);
            return RequestBasicInfoGetGetResult(basicInfo, ServiceSelected);
        }

        private RequestBasicInfoDataOutputDto RequestBasicInfoGetGetResult(RequestBasicInfoGetDto inputDto, IEnumerable<SelectionDto> ServiceSelected)
        {
            return new RequestBasicInfoDataOutputDto()
            {
                RegionId = inputDto.RegionId,
                RegionTitle = inputDto.RegionTitle,
                ZoneId = inputDto.ZoneId,
                ZoneTitle = inputDto.ZoneTitle,
                FirstName = inputDto.FirstName,
                Surname = inputDto.Surname,
                FullName = inputDto.FullName,
                FatherName = inputDto.FatherName,
                BranchTypeId = inputDto.BranchTypeId,
                BranchTypeTitle = inputDto.BranchTypeTitle,
                CustomerNumber = inputDto.CustomerNumber,
                ReadingNumber = inputDto.ReadingNumber,
                TrackNumber = inputDto.TrackNumber,
                Address = inputDto.Address,
                PostalCode = inputDto.PostalCode,
                NationalCode = inputDto.NationalCode,
                Description = inputDto.Description,
                RequestDateJalali = inputDto.RequestDateJalali,
                RegisterDateJalali = inputDto.RegisterDateJalali,
                DiscountCount = inputDto.DiscountCount,
                DiscountTypeId = inputDto.DiscountTypeId,
                DiscountTypeTitle = inputDto.DiscountTypeTitle,
                PhoneNumber = inputDto.PhoneNumber,
                NeighbourBillId = inputDto.NeighbourBillId,
                ServiceGroupId = inputDto.ServiceGroupId,
                ServiceGroupTitle = inputDto.ServiceGroupTitle,
                PreviousStatusId = inputDto.PreviousStatusId,
                PreviousStatusTitle = inputDto.PreviousStatusTitle,
                PreviousStepDateJalali = inputDto.PreviousStepDateJalali,
                BillId = inputDto.BillId,
                PreviousTrackId = inputDto.PreviousTrackId,
                ServiceSelected = ServiceSelected
            };
        }

        private MoshtrakServiceDto RequestBasicInfoGetGetMoshtrakService(RequestBasicInfoGetDto serviceSelected)
        {
            return new MoshtrakServiceDto()
            {
                s0 = serviceSelected.s0,
                s1 = serviceSelected.s1,
                s2 = serviceSelected.s2,
                s3 = serviceSelected.s3,
                s4 = serviceSelected.s4,
                s5 = serviceSelected.s5,
                s8 = serviceSelected.s8,
                s9 = serviceSelected.s9,
                s10 = serviceSelected.s10,
                s11 = serviceSelected.s11,
                s12 = serviceSelected.s12,
                s13 = serviceSelected.s13,
                s14 = serviceSelected.s14,
                s15 = serviceSelected.s15,
                s16 = serviceSelected.s16,
                s17 = serviceSelected.s17,
                s18 = serviceSelected.s18,
                s19 = serviceSelected.s19,
                s20 = serviceSelected.s20,
                s21 = serviceSelected.s21,
                s22 = serviceSelected.s22,
                s23 = serviceSelected.s23,
                s24 = serviceSelected.s24,
                s25 = serviceSelected.s25,
                s26 = serviceSelected.s26,
                s27 = serviceSelected.s27,
                s28 = serviceSelected.s28,
                s29 = serviceSelected.s29,
                s30 = serviceSelected.s30,
                s31 = serviceSelected.s31,
                s32 = serviceSelected.s32,
                s33 = serviceSelected.s33,
                s34 = serviceSelected.s34,
                s35 = serviceSelected.s35,
                s36 = serviceSelected.s36,
                s37 = serviceSelected.s37,
                s38 = serviceSelected.s38,
                s39 = serviceSelected.s39,
                s40 = serviceSelected.s40,
                s41 = serviceSelected.s41,
                s42 = serviceSelected.s42,
                s43 = serviceSelected.s43,
                s44 = serviceSelected.s44,
                s45 = serviceSelected.s45,
                s46 = serviceSelected.s46,
                s47 = serviceSelected.s47,
                s48 = serviceSelected.s48,
            };
        }

        public async Task<IEnumerable<PreviousRequestDataOutputDto>> PreviousRequestGetByBillIdAsync(string billId, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQueryService.Get(billId);
            IEnumerable<PreviousRequestGetDto> previousRequest = await _moshtrakQueryService.GetAllRequestByCustomerNumber(zoneIdAndCustomerNumber);
            ICollection<PreviousRequestDataOutputDto> result = new List<PreviousRequestDataOutputDto>();
            foreach (var item in previousRequest)
            {
                MoshtrakServiceDto sDto = PreviousRequestGetByBillIdGetMoshtrakDto(item);
                IEnumerable<SelectionDto> companyService = await _moshtrakService.GetMoshtrakCompanyServiceDto(sDto, item.ServiceGroupId);
                IEnumerable<string> companyServiceTitles = companyService.Where(c => c.IsSelected).Select(c => c.Title);
                PreviousRequestDataOutputDto outPut = PreviousRequestGetByBillIdGeOutput(item, string.Join(", ", companyServiceTitles));
                result.Add(outPut);
            }

            return result;
        }

        private MoshtrakServiceDto PreviousRequestGetByBillIdGetMoshtrakDto(PreviousRequestGetDto input)
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

        private PreviousRequestDataOutputDto PreviousRequestGetByBillIdGeOutput(PreviousRequestGetDto input, string requestsTitle)
        {
            return new PreviousRequestDataOutputDto()
            {
                BillId = input.BillId,
                CustomerNumber = input.CustomerNumber,
                ZoneId = input.ZoneId,
                ZoneTitle = input.ZoneTitle,
                RegionId = input.RegionId,
                RegionTitle = input.RegionTitle,
                UsageId = input.UsageId,
                UsageTitle = input.UsageTitle,
                TrackNumber = input.TrackNumber,
                StringTrackNumber = input.StringTrackNumber,
                RequestDateJalali = input.RequestDateJalali,
                RegisterDateJalali = input.RegisterDateJalali,
                RequestsTitle = requestsTitle,
            };
        }

        public ICollection<NumericDictionary> KitCodeDictionaryGetAsync(CancellationToken cancellationToken)
        {
            var kitCodeDictionary = new List<NumericDictionary>()
            {
                new NumericDictionary(1, "بسته لوازم انشعاب قطر 1/2 اینچ دارای کنتور مولتی جت خشک (محفظه زمینی)"),
                new NumericDictionary(2, "بسته لوازم انشعاب قطر 3/4 اینچ دارای کنتور مولتی جت خشک (محفظه زمینی)"),
                new NumericDictionary(3, "بسته لوازم انشعاب قطر 1 اینچ دارای کنتور مولتی جت خشک (محفظه زمینی)"),
                new NumericDictionary(4, "بسته لوازم انشعاب قطر 1,1/2 اینچ دارای کنتور مولتی جت خشک (محفظه زمینی)"),
                new NumericDictionary(5, "بسته لوازم انشعاب قطر 1/2 اینچ دارای کنتور هوشمند (محفظه زمینی)"),
                new NumericDictionary(6, "بسته لوازم انشعاب قطر 3/4 اینچ دارای کنتور هوشمند (محفظه زمینی)"),
                new NumericDictionary(7, "بسته لوازم انشعاب قطر 1 اینچ دارای کنتور هوشمند (محفظه زمینی)"),
                new NumericDictionary(8, "بسته لوازم انشعاب قطر 1,1/2 اینچ دارای کنتور هوشمند (محفظه زمینی)"),
                new NumericDictionary(9, "بسته لوازم انشعاب قطر 1/2 اینچ دارای کنتور خشک (تابلو محفظه دیواری)"),
                new NumericDictionary(10, "بسته لوازم انشعاب قطر 3/4 اینچ دارای کنتور خشک (تابلو محفظه دیواری)"),
                new NumericDictionary(11, "بسته لوازم انشعاب قطر 1 اینچ دارای کنتور خشک (تابلو محفظه دیواری)"),
                new NumericDictionary(12, "بسته لوازم انشعاب قطر 1/2 اینچ دارای کنتور خشک کنتوری برای 2 واحد (تابلو محفظه دیواری)"),
                new NumericDictionary(13, "بسته لوازم انشعاب قطر 3/4 اینچ دارای کنتور خشک کنتوری برای 3 واحد (تابلو محفظه دیواری)"),
                new NumericDictionary(14, "بسته لوازم انشعاب قطر 3/4 اینچ دارای کنتور خشک کنتوری برای 4 واحد (تابلو محفظه دیواری)"),
                new NumericDictionary(15, "بسته لوازم انشعاب قطر 1/2 اینچ دارای کنتور خشک کنتوری برای 2 واحد (محفظه زمینی)"),
                new NumericDictionary(16, "بسته لوازم انشعاب قطر 1/2 اینچ دارای کنتور خشک کنتوری برای 3 واحد (محفظه زمینی)"),
                new NumericDictionary(17, "بسته لوازم انشعاب قطر 3/4 اینچ دارای کنتور خشک کنتوری برای 4 واحد (محفظه زمینی)"),
            };
            return kitCodeDictionary;
        }

        public IEnumerable<StringDictionary> ReadingBlockListGetAsync(CancellationToken cancellationToken)
        {
            return[new StringDictionary("ندارد", "ندارد"), new StringDictionary("A", "A"), new StringDictionary("B", "B"), new StringDictionary("C", "C"), new StringDictionary("D", "D"), new StringDictionary("E", "E"), new StringDictionary("F", "F"), new StringDictionary("G", "G"), new StringDictionary("H", "H"), new StringDictionary("I", "I"), new StringDictionary("J", "J"), new StringDictionary("K", "K"), new StringDictionary("L", "L"), new StringDictionary("M", "M"), new StringDictionary("N", "N")];
        }

        public async Task<ToCalculationConfirmGetDto> ToCalulationConfirmAsync(Guid trackId, IAppUser appUser, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.Get(trackId);
            MoshtrakGetDto moshtrackSearch = new(trackingInfo.ZoneId, null, null, trackingInfo.TrackNumber);
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(moshtrackSearch, MoshtrakSearchTypeEnum.ByTrackNumber)).FirstOrDefault();
            TrackingOutputDto? latestAssessmentResult = await _trackingQueryService.GetLatest(trackingInfo.TrackNumber, _ToCalulationConfirmassessmentResultId, false);
            AssessmentDataOutputDto? assessmentInfo = latestAssessmentResult is null ? null : await _assessmentQueryService.GetByTrackId(latestAssessmentResult.TrackId, true);
            await _commonZoneService.IsUserInZone(appUser, trackingInfo.ZoneId);
            MoshtrakServiceDto sData = _moshtrakService.GetMoshtrakServiceDto(moshtrakInfo);
            IEnumerable<SelectionDto> companyServices = await _moshtrakService.GetMoshtrakCompanyServiceDto(sData, trackingInfo.ServiceGroupId);
            ToCalculationConfirmGetDto moshtrakData = ToCalulationConfirmGetData(moshtrakInfo, companyServices, trackingInfo, assessmentInfo);
            return moshtrakData;
        }

        private ToCalculationConfirmGetDto ToCalulationConfirmGetData(MoshtrakOutputDto moshtrakInfo, IEnumerable<SelectionDto> companyServices, TrackingOutputDto trackingInfo, AssessmentDataOutputDto? assessmentInfo)
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
                AssessmentCode = assessmentInfo?.AssessmentCode ?? null,
                AssessmentDateJalali = assessmentInfo?.AssessmentDateJalali ?? null,
                AssessmentMobile = assessmentInfo?.AssessmentMobile ?? null,
                AssessmentName = assessmentInfo?.AssessmentName ?? null,
                SelectedServices = companyServices,
            };
        }

        public async Task<ToSetAssessmentTimeGetDto> ToSetAssessmentTimeGetByTrackIdAsync(Guid trackId, IAppUser appUser, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.Get(trackId);
            await _commonZoneService.IsUserInZone(appUser, trackingInfo.ZoneId);
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, trackingInfo.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber, true)).FirstOrDefault();
            //IEnumerable<StringDictionary> assessmentsList = await _assessmentGetByZoneIdHandler.Handle(moshtrakInfo.ZoneId, cancellationToken);
            ICollection<UserQueryDto> readerInfoList = await _userRoleQueryService.GetByZone(_ToSetAssessmentTimeGetByTrackIdreaderRoleId, moshtrakInfo.ZoneId);
            IEnumerable<StringDictionary> assessmentsList = readerInfoList.Select(x => new StringDictionary(x.Username, x.DisplayName)).ToList();
            MoshtrakServiceDto moshtrakServiceDto = ToSetAssessmentTimeGetByTrackIdGetMoshtrakService(moshtrakInfo);
            IEnumerable<NumericDictionary> serviceSelected = await _moshtrakService.GetServicesSelectedDto(moshtrakServiceDto, trackingInfo.ServiceGroupId);
            ToSetAssessmentTimeGetDto result = new()
            {
                RegionId = moshtrakInfo.RegionId,
                RegionTitle = moshtrakInfo.RegionTitle,
                ZoneId = moshtrakInfo.ZoneId,
                ZoneTitle = moshtrakInfo.ZoneTitle,
                TrackId = trackingInfo.TrackId,
                TrackNumber = trackingInfo.TrackNumber,
                BillId = trackingInfo.BillId ?? string.Empty,
                NeighbourBillId = moshtrakInfo.NeighbourBillId ?? string.Empty,
                NationalCode = moshtrakInfo.NeighbourBillId,
                Address = moshtrakInfo.Address,
                NotificationNumber = moshtrakInfo.NotificationMobile,
                MobileNumber = moshtrakInfo.MobileNumber,
                FirstName = moshtrakInfo.FirstName,
                Surname = moshtrakInfo.Surname,
                AssessmentsList = assessmentsList,
                ServiceSelected = serviceSelected,
            };
            return result;
        }

        private MoshtrakServiceDto ToSetAssessmentTimeGetByTrackIdGetMoshtrakService(MoshtrakOutputDto serviceSelected)
        {
            return new MoshtrakServiceDto()
            {
                s0 = serviceSelected.s0,
                s1 = serviceSelected.s1,
                s2 = serviceSelected.s2,
                s3 = serviceSelected.s3,
                s4 = serviceSelected.s4,
                s5 = serviceSelected.s5,
                s8 = serviceSelected.s8,
                s9 = serviceSelected.s9,
                s10 = serviceSelected.s10,
                s11 = serviceSelected.s11,
                s12 = serviceSelected.s12,
                s13 = serviceSelected.s13,
                s14 = serviceSelected.s14,
                s15 = serviceSelected.s15,
                s16 = serviceSelected.s16,
                s17 = serviceSelected.s17,
                s18 = serviceSelected.s18,
                s19 = serviceSelected.s19,
                s20 = serviceSelected.s20,
                s21 = serviceSelected.s21,
                s22 = serviceSelected.s22,
                s23 = serviceSelected.s23,
                s24 = serviceSelected.s24,
                s25 = serviceSelected.s25,
                s26 = serviceSelected.s26,
                s27 = serviceSelected.s27,
                s28 = serviceSelected.s28,
                s29 = serviceSelected.s29,
                s30 = serviceSelected.s30,
                s31 = serviceSelected.s31,
                s32 = serviceSelected.s32,
                s33 = serviceSelected.s33,
                s34 = serviceSelected.s34,
                s35 = serviceSelected.s35,
                s36 = serviceSelected.s36,
                s37 = serviceSelected.s37,
                s38 = serviceSelected.s38,
                s39 = serviceSelected.s39,
                s40 = serviceSelected.s40,
                s41 = serviceSelected.s41,
                s42 = serviceSelected.s42,
                s43 = serviceSelected.s43,
                s44 = serviceSelected.s44,
                s45 = serviceSelected.s45,
                s46 = serviceSelected.s46,
                s47 = serviceSelected.s47,
                s48 = serviceSelected.s48,
            };
        }

        public async Task<ToSetReAssessmentTimeGetDto> ToSetReAssessmentTimeGetByTrackIdAsync(Guid trackId, IAppUser appUser, CancellationToken cancellationToken)
        {
            TrackingOutputDto trackingInfo = await _trackingQueryService.Get(trackId);
            await _commonZoneService.IsUserInZone(appUser, trackingInfo.ZoneId);
            MoshtrakOutputDto moshtrakInfo = (await _moshtrakQueryService.Get(new MoshtrakGetDto(trackingInfo.ZoneId, null, null, trackingInfo.TrackNumber), MoshtrakSearchTypeEnum.ByTrackNumber, true)).FirstOrDefault();
            IEnumerable<StringDictionary> assessmentsList = await _assessmentGetByZoneIdHandler.Handle(appUser, cancellationToken);
            MoshtrakServiceDto moshtrakServiceDto = ToSetReAssessmentTimeGetByTrackIdGetMoshtrakService(moshtrakInfo);
            IEnumerable<NumericDictionary> serviceSelected = await _moshtrakService.GetServicesSelectedDto(moshtrakServiceDto, trackingInfo.ServiceGroupId);
            ToSetReAssessmentTimeGetDto result = new()
            {
                RegionId = moshtrakInfo.RegionId,
                RegionTitle = moshtrakInfo.RegionTitle,
                ZoneId = moshtrakInfo.ZoneId,
                ZoneTitle = moshtrakInfo.ZoneTitle,
                TrackId = trackingInfo.TrackId,
                TrackNumber = trackingInfo.TrackNumber,
                BillId = trackingInfo.BillId ?? string.Empty,
                NeighbourBillId = moshtrakInfo.NeighbourBillId ?? string.Empty,
                NationalCode = moshtrakInfo.NeighbourBillId,
                Address = moshtrakInfo.Address,
                NotificationNumber = moshtrakInfo.NotificationMobile,
                MobileNumber = moshtrakInfo.MobileNumber,
                FirstName = moshtrakInfo.FirstName,
                Surname = moshtrakInfo.Surname,
                AssessmentsList = assessmentsList,
                ServiceSelected = serviceSelected,
            };
            return result;
        }

        private MoshtrakServiceDto ToSetReAssessmentTimeGetByTrackIdGetMoshtrakService(MoshtrakOutputDto serviceSelected)
        {
            return new MoshtrakServiceDto()
            {
                s0 = serviceSelected.s0,
                s1 = serviceSelected.s1,
                s2 = serviceSelected.s2,
                s3 = serviceSelected.s3,
                s4 = serviceSelected.s4,
                s5 = serviceSelected.s5,
                s8 = serviceSelected.s8,
                s9 = serviceSelected.s9,
                s10 = serviceSelected.s10,
                s11 = serviceSelected.s11,
                s12 = serviceSelected.s12,
                s13 = serviceSelected.s13,
                s14 = serviceSelected.s14,
                s15 = serviceSelected.s15,
                s16 = serviceSelected.s16,
                s17 = serviceSelected.s17,
                s18 = serviceSelected.s18,
                s19 = serviceSelected.s19,
                s20 = serviceSelected.s20,
                s21 = serviceSelected.s21,
                s22 = serviceSelected.s22,
                s23 = serviceSelected.s23,
                s24 = serviceSelected.s24,
                s25 = serviceSelected.s25,
                s26 = serviceSelected.s26,
                s27 = serviceSelected.s27,
                s28 = serviceSelected.s28,
                s29 = serviceSelected.s29,
                s30 = serviceSelected.s30,
                s31 = serviceSelected.s31,
                s32 = serviceSelected.s32,
                s33 = serviceSelected.s33,
                s34 = serviceSelected.s34,
                s35 = serviceSelected.s35,
                s36 = serviceSelected.s36,
                s37 = serviceSelected.s37,
                s38 = serviceSelected.s38,
                s39 = serviceSelected.s39,
                s40 = serviceSelected.s40,
                s41 = serviceSelected.s41,
                s42 = serviceSelected.s42,
                s43 = serviceSelected.s43,
                s44 = serviceSelected.s44,
                s45 = serviceSelected.s45,
                s46 = serviceSelected.s46,
                s47 = serviceSelected.s47,
                s48 = serviceSelected.s48,
            };
        }
    }
}
