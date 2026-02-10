using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.ClaimPool.Persistence.Features.Land.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Metering.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.People.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Features.WasteWater.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class AssessmentTaskGetAllHandler : IAssessmentTaskGetAllHandler
    {
        private readonly IAssessmentTaskQueryService _assessmentTaskQueryService;
        private readonly ITrackingResultQueryService _trackingResultQueryService;
        private readonly IUsageQuerySevice _usageQueryService;
        private readonly IHandoverQueryService _handoverQueryService;
        private readonly IMeterDiameterQueryService _meterDiameterQueryService;
        private readonly ISiphonDiameterQueryService _siphonDiameterQueryService;
        private readonly IDiscountTypeQueryService _discountTypeQueryService;
        private readonly ICompanyServiceQueryService _companyServiceQueryService;

        public AssessmentTaskGetAllHandler(
            IAssessmentTaskQueryService assessmentTaskQueryService,
            ITrackingResultQueryService trackingResultQueryService,
            IUsageQuerySevice usageQueryService,
            IHandoverQueryService handoverQueryService,
            IMeterDiameterQueryService meterDiameterQueryService,
            ISiphonDiameterQueryService siphonDiameterQueryService,
            IDiscountTypeQueryService discountTypeQueryService,
            ICompanyServiceQueryService companyServiceQueryService)
        {
            _assessmentTaskQueryService = assessmentTaskQueryService;
            _assessmentTaskQueryService.NotNull(nameof(assessmentTaskQueryService));

            _trackingResultQueryService = trackingResultQueryService;
            _trackingResultQueryService.NotNull(nameof(trackingResultQueryService));

            _usageQueryService = usageQueryService;
            _usageQueryService.NotNull(nameof(usageQueryService));

            _handoverQueryService = handoverQueryService;
            _handoverQueryService.NotNull(nameof(handoverQueryService));

            _meterDiameterQueryService = meterDiameterQueryService;
            _meterDiameterQueryService.NotNull(nameof(meterDiameterQueryService));

            _siphonDiameterQueryService = siphonDiameterQueryService;
            _siphonDiameterQueryService.NotNull(nameof(siphonDiameterQueryService));

            _discountTypeQueryService = discountTypeQueryService;
            _discountTypeQueryService.NotNull(nameof(discountTypeQueryService));

            _companyServiceQueryService = companyServiceQueryService;
            _companyServiceQueryService.NotNull(nameof(companyServiceQueryService));
        }

        public async Task<AssessmentTasksOutputDto> Handle(int assessmentCode, CancellationToken cancellationToken)
        {
            IEnumerable<Common.BaseEntities.NumericDictionary> trackingResultsDictionary = await _trackingResultQueryService.Get();

            ICollection<Usage> usageList = await _usageQueryService.Get();
            IEnumerable<Common.BaseEntities.NumericDictionary> usagesDictionary = usageList.Select(u => new Common.BaseEntities.NumericDictionary(u.Id, u.Title));

            ICollection<Handover> branchTypeList = await _handoverQueryService.Get();
            IEnumerable<Common.BaseEntities.NumericDictionary> branchTypeDictionary = branchTypeList.Select(b => new Common.BaseEntities.NumericDictionary(b.Id, b.Title));

            ICollection<MeterDiameter> meterDiameterList = await _meterDiameterQueryService.Get();
            IEnumerable<Common.BaseEntities.NumericDictionary> meterDiameterDictionary = meterDiameterList.Select(m => new Common.BaseEntities.NumericDictionary(m.Id, m.Title));

            ICollection<SiphonDiameter> siphonDiameterList = await _siphonDiameterQueryService.Get();
            IEnumerable<NumericDictionary> siphonDiameterDictionary = siphonDiameterList.Select(s => new NumericDictionary(s.Id, s.Title));

            ICollection<DiscountType> discountTypeList = await _discountTypeQueryService.Get();
            IEnumerable<Common.BaseEntities.NumericDictionary> discountTypeDictionary = discountTypeList.Select(d => new Common.BaseEntities.NumericDictionary((int)d.Id, d.Title));

            IEnumerable<AssessmentLocationInfoOutputDto> locationsInfo = await GetLocationsInfo(assessmentCode);

            return new AssessmentTasksOutputDto()
            {
                LocationsInfo = locationsInfo,
                Usages = usagesDictionary,
                BranchTypes = branchTypeDictionary,
                TrackingResults = trackingResultsDictionary,
                MeterDiameters = meterDiameterDictionary,
                SiphonDiameters = siphonDiameterDictionary,
                DiscountTypes = discountTypeDictionary,
            };
        }
        private async Task<IEnumerable<AssessmentLocationInfoOutputDto>> GetLocationsInfo(int assessmentCode)
        {
            IEnumerable<GuidDictionary> tracksDictionary = await _assessmentTaskQueryService.Get(assessmentCode);
            ICollection<AssessmentLocationInfoOutputDto> locationsInfoResult = new List<AssessmentLocationInfoOutputDto>();

            var tracksGroup = tracksDictionary.GroupBy(t => t.Id).ToList();
            foreach (var track in tracksGroup)
            {
                int zoneId = track.Key;
                IEnumerable<Guid> trackIds = track.Select(t => t.Title).ToList();

                IEnumerable<AssessmentLocationInfoWithSOutputDto> locationsInfoWithS = await _assessmentTaskQueryService.GetLocationsInfo(trackIds, zoneId);
                foreach (var locationInfo in locationsInfoWithS)
                {
                    AssessmentLocationInfoOutputDto info = await GetAssessmentLocationInfo(locationInfo);
                    locationsInfoResult.Add(info);
                }
            }
            return locationsInfoResult;
        }
        private async Task<IEnumerable<ServiceGroupWithCheckedOutputDto>> GetItemsService(AssessmentLocationInfoWithSOutputDto inputDto)
        {
            IEnumerable<Common.BaseEntities.NumericDictionary> itemsService = await _companyServiceQueryService.GetByTypeId(inputDto.ServiceGroupId);
            ICollection<ServiceGroupWithCheckedOutputDto> itemsServiceWithChecked = new List<ServiceGroupWithCheckedOutputDto>();

            if (inputDto.ServiceGroupId == 1)//Sale
            {
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.IsEnsheabAb, CompanySeviceLiterals.IsEnsheabAb, inputDto.s0 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.IsEnsheabFazelab, CompanySeviceLiterals.IsEnsheabFazelab, inputDto.s1 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.NezamMohandesi, CompanySeviceLiterals.NezamMohandesi, inputDto.s37 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.IsAmadeSaziAb, CompanySeviceLiterals.IsAmadeSaziAb, inputDto.s26 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.IsAmadeSaziFazelab, CompanySeviceLiterals.IsAmadeSaziFazelab, inputDto.s27 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.LooleGozareAbFazelab, CompanySeviceLiterals.LooleGozareAbFazelab, inputDto.s43 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.KontorMojaza, CompanySeviceLiterals.KontorMojaza, inputDto.s45 > 0 ? true : false));
            }
            else//AfterSale
            {
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.IsEnsheabFazelab, CompanySeviceLiterals.IsEnsheabFazelab, inputDto.s1 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.IsTaqirNam, CompanySeviceLiterals.IsTaqirNam, inputDto.s4 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.IsTaqirVahed, CompanySeviceLiterals.IsTaqirVahed, inputDto.s2 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.IsTaqirKarbari, CompanySeviceLiterals.IsTaqirKarbari, inputDto.s16 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.IsTaqirQotrEnsheab, CompanySeviceLiterals.IsTaqirQotrEnsheab, inputDto.s5 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.SifoonEzafe, CompanySeviceLiterals.SifoonEzafe, inputDto.s33 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.JabejaiiKontor, CompanySeviceLiterals.JabejaiiKontor, inputDto.s20 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.TafkikEdqam, CompanySeviceLiterals.TafkikEdqam, inputDto.s40 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.TaqirQotrSifoon, CompanySeviceLiterals.TaqirQotrSifoon, inputDto.s24 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.EstelamMahzar, CompanySeviceLiterals.EstelamMahzar, inputDto.s10 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.QatVaslEnsheab, CompanySeviceLiterals.QatVaslEnsheab, inputDto.s32 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.JabejaiiSifoon, CompanySeviceLiterals.JabejaiiSifoon, inputDto.s36 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.NezamMohandesi, CompanySeviceLiterals.NezamMohandesi, inputDto.s37 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.TavizKontor, CompanySeviceLiterals.TavizKontor, inputDto.s41 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.IsZarfiatQarardadi, CompanySeviceLiterals.IsZarfiatQarardadi, inputDto.s44 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.IsAmadeSaziAb, CompanySeviceLiterals.IsAmadeSaziAb, inputDto.s26 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.IsAmadeSaziFazelab, CompanySeviceLiterals.IsAmadeSaziFazelab, inputDto.s27 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.LooleGozareAbFazelab, CompanySeviceLiterals.LooleGozareAbFazelab, inputDto.s43 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.KontorMojaza, CompanySeviceLiterals.KontorMojaza, inputDto.s45 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.KhanevarShomari, CompanySeviceLiterals.KhanevarShomari, inputDto.s39 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.TaqirTarefe, CompanySeviceLiterals.TaqirTarefe, inputDto.s46 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.Peymayesh, CompanySeviceLiterals.Peymayesh, inputDto.s47 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.Saier, CompanySeviceLiterals.Saier, inputDto.s48 > 0 ? true : false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.TaqirSathCounter, CompanySeviceLiterals.TaqirSathCounter, inputDto.s13 > 0 ? true : false));

            }

            return itemsServiceWithChecked;
        }
        private async Task<AssessmentLocationInfoOutputDto> GetAssessmentLocationInfo(AssessmentLocationInfoWithSOutputDto locationInfoWithS)
        {
            IEnumerable<ServiceGroupWithCheckedOutputDto> items = await GetItemsService(locationInfoWithS);
            AssessmentLocationInfoOutputDto locationInfo = new()
            {
                MobileNumber = locationInfoWithS.MobileNumber,
                PhoneNumber = locationInfoWithS.PhoneNumber,
                NotificationMobileNumber = locationInfoWithS.NotificationMobileNumber,
                BillId = locationInfoWithS.BillId,
                NeighbourBillId = locationInfoWithS.NeighbourBillId,
                StringTrackNumber = locationInfoWithS.StringTrackNumber,
                TrackNumber=locationInfoWithS.TrackNumber,
                CustomerNumber = locationInfoWithS.CustomerNumber,
                ServiceGroupId = locationInfoWithS.ServiceGroupId,
                ServiceGroupTitle = locationInfoWithS.ServiceGroupTitle,
                FirstName = locationInfoWithS.FirstName,
                Surname = locationInfoWithS.Surname,
                ZoneId = locationInfoWithS.ZoneId,
                ZoneTitle = locationInfoWithS.ZoneTitle,
                DiscountCount = locationInfoWithS.DiscountCount,
                NationalCode = locationInfoWithS.NationalCode,
                FatherName = locationInfoWithS.FatherName,
                HouseValue = locationInfoWithS.HouseValue,
                Description = locationInfoWithS.Description,
                IsNonPermanent = locationInfoWithS.IsNonPermanent,
                HasCustomerNumber = locationInfoWithS.HasCustomerNumber,
                FullName = locationInfoWithS.FullName,
                IsVisited = locationInfoWithS.IsVisited,
                DiscountTitle = locationInfoWithS.DiscountTitle,
                UsageId = locationInfoWithS.UsageId,
                UsageTitle = locationInfoWithS.UsageTitle,
                MeterDiameterId = locationInfoWithS.MeterDiameterId,
                MeterDiameterTitle = locationInfoWithS.MeterDiameterTitle,
                AssessmentDateJalali = locationInfoWithS.AssessmentDateJalali,
                AssessmentCode = locationInfoWithS.AssessmentCode,
                AssessmentMobileNumber = locationInfoWithS.AssessmentMobileNumber,
                AssessmentName = locationInfoWithS.AssessmentName,
                ServiceGroups = items
            };

            return locationInfo;
        }
    }
}