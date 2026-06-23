using Aban360.BlobPool.Persistence.Features.DmsServices.Queries.Contracts;
using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using Aban360.ClaimPool.Domain.Features.People.Entities;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Commands;
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
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Implementations;

namespace Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Implementations
{
    internal sealed class AssessmentTaskGetAllHandler : IAssessmentTaskGetAllHandler
    {
        private readonly IAssessmentTaskQueryService _assessmentTaskQueryService;
        private readonly IT64QueryService _trackingResultQueryService;
        private readonly IUsageQuerySevice _usageQueryService;
        private readonly IHandoverQueryService _handoverQueryService;
        private readonly IMeterDiameterQueryService _meterDiameterQueryService;
        private readonly ISiphonDiameterQueryService _siphonDiameterQueryService;
        private readonly IDiscountTypeQueryService _discountTypeQueryService;
        private readonly ICompanyServiceQueryService _companyServiceQueryService;
        private readonly IMeterMaterialQueryService _meterMaterialQueryService;
        private readonly IOpenKmMetaDataQueryServices _openKmMetaDataQueryService;
        private readonly IGuildQueryHandler _guildQueryHandler;
        private readonly IT9QueryService _t9QueryService;
        private int _saleServiceGroupId = 1;
        private int _afterSaleServiceGroupId = 2;
        public AssessmentTaskGetAllHandler(
            IAssessmentTaskQueryService assessmentTaskQueryService,
            IT64QueryService trackingResultQueryService,
            IUsageQuerySevice usageQueryService,
            IHandoverQueryService handoverQueryService,
            IMeterDiameterQueryService meterDiameterQueryService,
            ISiphonDiameterQueryService siphonDiameterQueryService,
            IDiscountTypeQueryService discountTypeQueryService,
            ICompanyServiceQueryService companyServiceQueryService,
            IMeterMaterialQueryService meterMaterialQueryService,
            IOpenKmMetaDataQueryServices openKmMetaDataQueryService,
            IGuildQueryHandler guildQueryHandler,
            IT9QueryService t9QueryService)
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

            _meterMaterialQueryService = meterMaterialQueryService;
            _meterMaterialQueryService.NotNull(nameof(meterMaterialQueryService));

            _openKmMetaDataQueryService = openKmMetaDataQueryService;
            _openKmMetaDataQueryService.NotNull(nameof(openKmMetaDataQueryService));

            _guildQueryHandler = guildQueryHandler;
            _guildQueryHandler.NotNull(nameof(guildQueryHandler));

            _t9QueryService = t9QueryService;
            _t9QueryService.NotNull(nameof(t9QueryService));
        }

        public async Task<AssessmentTasksOutputDto> Handle(int assessmentCode, CancellationToken cancellationToken)
        {
            IEnumerable<AssessmentResultByPreResultOutputDto> trackingResultsDictionary = await _trackingResultQueryService.GetAssessment();

            ICollection<Usage> usageList = await _usageQueryService.Get();
            IEnumerable<NumericDictionary> usagesDictionary = usageList.Select(u => new NumericDictionary(u.Id, u.Title));

            ICollection<Handover> branchTypeList = await _handoverQueryService.Get();
            IEnumerable<NumericDictionary> branchTypeDictionary = branchTypeList.Select(b => new NumericDictionary(b.Id, b.Title));

            ICollection<MeterDiameter> meterDiameterList = await _meterDiameterQueryService.Get();
            IEnumerable<NumericDictionary> meterDiameterDictionary = meterDiameterList.Select(m => new NumericDictionary(m.Id, m.Title));

            ICollection<SiphonDiameter> siphonDiameterList = await _siphonDiameterQueryService.Get();
            IEnumerable<NumericDictionary> siphonDiameterDictionary = siphonDiameterList.Select(s => new NumericDictionary(s.Id, s.Title));

            ICollection<DiscountType> discountTypeList = await _discountTypeQueryService.Get();
            IEnumerable<NumericDictionary> discountTypeDictionary = discountTypeList.Select(d => new NumericDictionary((int)d.Id, d.Title));

            ICollection<MeterMaterial> meterMaterialList = await _meterMaterialQueryService.Get();
            IEnumerable<NumericDictionary> meterMaterialDictionary = meterMaterialList.Select(m => new NumericDictionary(m.Id, m.Title));

            IEnumerable<NumericDictionary> archiveFileTypesDictionary = await _openKmMetaDataQueryService.GetFileTitles();

            IEnumerable<StringDictionary> BlockCodeDictionary = GetBlockCodes();

            IEnumerable<NumericDictionary> guildDictionary = await _guildQueryHandler.Handle(cancellationToken);

            IEnumerable<AssessmentLocationInfoOutputDto> locationsInfo = await GetLocationsInfo(assessmentCode);

            IEnumerable<NumericDictionary> saleServiceGroups = await _t9QueryService.GetByTypeId(_saleServiceGroupId);

            IEnumerable<NumericDictionary> afterSaleServiceGroups = await _t9QueryService.GetByTypeId(_afterSaleServiceGroupId);

            IEnumerable<StringDictionary> mapDictionary = GetMapDictionary();

            return new AssessmentTasksOutputDto()
            {
                LocationsInfo = locationsInfo,
                Usages = usagesDictionary,
                BranchTypes = branchTypeDictionary,
                TrackingResults = trackingResultsDictionary,
                MeterDiameters = meterDiameterDictionary,
                SiphonDiameters = siphonDiameterDictionary,
                DiscountTypes = discountTypeDictionary,
                MeterMaterials = meterMaterialDictionary,
                ArchiveFileTypes = archiveFileTypesDictionary,
                BlockCodes = BlockCodeDictionary,
                Guilds = guildDictionary,
                MapDictionary = mapDictionary,
                SaleServiceGroups = saleServiceGroups,
                AfterSaleServiceGroups = afterSaleServiceGroups,
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
            ICollection<ServiceGroupWithCheckedOutputDto> itemsServiceWithChecked = new List<ServiceGroupWithCheckedOutputDto>();

            if (inputDto.ServiceGroupId == 1)//Sale
            {
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.IsEnsheabAb, CompanySeviceLiterals.IsEnsheabAb, inputDto.HasEnsheabAb));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.IsSaleEnsheabFazelab, CompanySeviceLiterals.IsEnsheabFazelab, inputDto.HasEnsheabFazelab));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.SaleNezamMohandesi, CompanySeviceLiterals.NezamMohandesi, inputDto.HasNezamMohandesi));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.LooleGozariAb, CompanySeviceLiterals.LooleGozariAb, inputDto.HasLooleGozareAb));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.KontorMojaza, CompanySeviceLiterals.KontorMojaza, inputDto.HasKontorMojaza));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.SaleIsAmadeSaziAb, CompanySeviceLiterals.IsAmadeSaziAb, inputDto.HasAmadeSaziAb));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.SaleIsAmadeSaziFazelab, CompanySeviceLiterals.IsAmadeSaziFazelab, inputDto.HazAmadeSaziFazelab));
            }
            else//AfterSale
            {
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.IsAfterSaleEnsheabFazelab, CompanySeviceLiterals.IsEnsheabFazelab, inputDto.HasEnsheabFazelab));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.IsTaqirNam, CompanySeviceLiterals.IsTaqirNam, inputDto.HasTaqirName));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.IsTaqirVahed, CompanySeviceLiterals.IsTaqirVahed, inputDto.HasTaqirVahed));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.IsTaqirKarbari, CompanySeviceLiterals.IsTaqirKarbari, inputDto.HasTaqirKarbari));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.IsTaqirQotrEnsheab, CompanySeviceLiterals.IsTaqirQotrEnsheab, inputDto.HasTaqirQotrEnsheab));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.SifoonEzafe, CompanySeviceLiterals.SifoonEzafe, inputDto.HasSifoonEzafe));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.JabejaiiKontor, CompanySeviceLiterals.JabejaiiKontor, inputDto.HasJabejaiiKontor));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.BarchidanEnsheab, CompanySeviceLiterals.IsBarchidanEnsheab, false));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.TajmiEdqam, CompanySeviceLiterals.TajmiEdqam, inputDto.HasTafkikEdqam));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.TaqirQotrSifoon, CompanySeviceLiterals.TaqirQotrSifoon, inputDto.HasTaqirQotrSifoon));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.EstelamMahzar, CompanySeviceLiterals.EstelamMahzar, inputDto.HasEstelamMahzar));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.QatVaslEnsheab, CompanySeviceLiterals.QatVaslEnsheab, inputDto.HasQatVaslEnsheab));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.JabejaiiSifoon, CompanySeviceLiterals.JabejaiiSifoon, inputDto.HasJabejaiiSifoon));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.AfterSaleNezamMohandesi, CompanySeviceLiterals.NezamMohandesi, inputDto.HasNezamMohandesi));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.TavizKontor, CompanySeviceLiterals.TavizKontor, inputDto.HasTavizKontor));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.IsZarfiatQarardadi, CompanySeviceLiterals.IsZarfiatQarardadi, inputDto.HasZarfiatQarardadi));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.KhanevarShomari, CompanySeviceLiterals.KhanevarShomari, inputDto.HasKhanevarShomari));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.AfterSaleIsAmadeSaziAb, CompanySeviceLiterals.IsAmadeSaziAb, inputDto.HasAmadeSaziAb));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.AfterSaleIsAmadeSaziFazelab, CompanySeviceLiterals.IsAmadeSaziFazelab, inputDto.HazAmadeSaziFazelab));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.TaqirTarefe, CompanySeviceLiterals.TaqirTarefe, inputDto.HasTaqirTarefe));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.Peymayesh, CompanySeviceLiterals.Peymayesh, inputDto.HasPeymayesh));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.Saier, CompanySeviceLiterals.Saier, inputDto.HasSaier));
                itemsServiceWithChecked.Add(new ServiceGroupWithCheckedOutputDto((int)Domain.Constants.CompanyServiceEnum.TaqirSathCounter, CompanySeviceLiterals.TaqirSathCounter, inputDto.HasTaqirSathCounter));

            }

            return itemsServiceWithChecked;
        }
        private MoshtrakServiceDto GetSDto(AssessmentLocationInfoWithSOutputDto input)
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
        private async Task<AssessmentLocationInfoOutputDto> GetAssessmentLocationInfo(AssessmentLocationInfoWithSOutputDto locationInfoWithS)
        {
            IEnumerable<ServiceGroupWithCheckedOutputDto> items = await GetItemsService(locationInfoWithS);
            AssessmentLocationInfoOutputDto locationInfo = new()
            {
                TrackId = locationInfoWithS.TrackId,
                MobileNumber = locationInfoWithS.MobileNumber,
                PhoneNumber = locationInfoWithS.PhoneNumber,
                NotificationMobileNumber = locationInfoWithS.NotificationMobileNumber,
                BillId = locationInfoWithS.BillId,
                NeighbourBillId = locationInfoWithS.NeighbourBillId,
                StringTrackNumber = locationInfoWithS.StringTrackNumber,
                TrackNumber = locationInfoWithS.TrackNumber,
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
                ServiceGroups = items,
                CertificateNumber = locationInfoWithS.CertificateNumber,
                Address = locationInfoWithS.Address,
                PostalCode = locationInfoWithS.PostalCode,
                ReadingNumber = locationInfoWithS.ReadingNumber,
                BranchTypeTitle = locationInfoWithS.BranchTypeTitle,
                BranchTypeId = locationInfoWithS.BranchTypeId,
                ContractualCapacity = locationInfoWithS.ContractualCapacity,
                Premises = locationInfoWithS.Premises,
                ImprovementCommercial = locationInfoWithS.ImprovementCommercial,
                ImprovementDomestic = locationInfoWithS.ImprovementDomestic,
                ImprovementOverall = locationInfoWithS.ImprovementOverall,
                CommercialUnit = locationInfoWithS.CommercialUnit,
                DomesticUnit = locationInfoWithS.DomesticUnit,
                OtherUnit = locationInfoWithS.OtherUnit,
                LicenseIssuanceDateJalali = locationInfoWithS.LicenseIssuanceDateJalali,
                BlockCode = locationInfoWithS.BlockCode,
                Siphon100 = locationInfoWithS.Siphon100,
                Siphon125 = locationInfoWithS.Siphon125,
                Siphon150 = locationInfoWithS.Siphon150,
                Siphon200 = locationInfoWithS.Siphon200,
                MainSiphon = locationInfoWithS.MainSiphon,
                TrenchLenS = locationInfoWithS.TrenchLenS,
                TrenchLenW = locationInfoWithS.TrenchLenW,
                AsphaltLenS = locationInfoWithS.AsphaltLenS,
                AsphaltLenW = locationInfoWithS.AsphaltLenW,
                RockyLenS = locationInfoWithS.RockyLenS,
                RockyLenW = locationInfoWithS.RockyLenW,
                OtherLenS = locationInfoWithS.OtherLenS,
                OtherLenW = locationInfoWithS.OtherLenW,
                BasementDepth = locationInfoWithS.BasementDepth,
            };

            return locationInfo;
        }
        private IEnumerable<StringDictionary> GetBlockCodes()
        {
            return [
                    new StringDictionary("ندارد","ندارد"),
                    new StringDictionary("A", "A"),
                    new StringDictionary("B", "B"),
                    new StringDictionary("C", "C"),
                    new StringDictionary("D", "D"),
                    new StringDictionary("E", "E"),
                    new StringDictionary("F", "F"),
                    new StringDictionary("G", "G"),
                    new StringDictionary("H", "H"),
                    new StringDictionary("I", "I"),
                    new StringDictionary("J", "J"),
                    new StringDictionary("K", "K"),
                    new StringDictionary("L", "L"),
                    new StringDictionary("M", "M"),
                    new StringDictionary("N", "N")];
        }
        private IEnumerable<StringDictionary> GetMapDictionary()
        {
            return [new ("ماهواره","https://maps.abfaisfahan.com/api/tile/googlesatellite/"),
                new ("ناهمواری","https://maps.abfaisfahan.com/api/tile/googleterrain/"),
                new ("مسیر‌ها","https://maps.abfaisfahan.com/api/tile/osm/"),
                new ("گوگل","https://maps.abfaisfahan.com/api/tile/googlestreet/"),
                new ("هیبرید","https://maps.abfaisfahan.com/api/tile/googlehybrid/")];
        }
    }
}