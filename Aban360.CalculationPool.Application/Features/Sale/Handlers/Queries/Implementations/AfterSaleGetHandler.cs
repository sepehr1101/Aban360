using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Implementations
{
    public class AfterSaleGetHandler : IAfterSaleGetHandler
    {
        private readonly ISaleGetHandler _saleGetHandler;
        private readonly IEquipmentBrokerAndZoneQueryService _equipmentBrokerAndZoneQueryService;
        private readonly IOfferingQueryService _offeringQueryService;
        private static string _title = "پس از فروش";
        public AfterSaleGetHandler(
            ISaleGetHandler saleGetHandler,
            IEquipmentBrokerAndZoneQueryService equipmentBrokerAndZoneQueryService,
            IOfferingQueryService offeringQueryService)
        {
            _saleGetHandler = saleGetHandler;
            _saleGetHandler.NotNull(nameof(saleGetHandler));

            _equipmentBrokerAndZoneQueryService = equipmentBrokerAndZoneQueryService;
            _equipmentBrokerAndZoneQueryService.NotNull(nameof(equipmentBrokerAndZoneQueryService));

            _offeringQueryService = offeringQueryService;
            _offeringQueryService.NotNull(nameof(offeringQueryService));
        }

        public async Task<FlatReportOutput<SaleHeaderOutputDto, AfterSaleDataOutputDto>> Handle(AfterSaleInputDto input, CancellationToken cancellationToken)
        {
            //ToDo : ValidationDto
            ValidationOffering(input);

            SaleInputDto previousDataInput = GetPreviousSaleInput(input);
            SaleInputDto currentDataInput = GetCurrentSaleInput(input);

            ReportOutput<SaleHeaderOutputDto, SaleDataOutputDto> previousSaleCalculation = await _saleGetHandler.Handle(previousDataInput, cancellationToken);
            ReportOutput<SaleHeaderOutputDto, SaleDataOutputDto> currentSaleCalculation = await _saleGetHandler.Handle(currentDataInput, cancellationToken);

            //  IEnumerable<SaleDataOutputDto> differentData = GetDifferentData(previousSaleCalculation.ReportData, currentSaleCalculation.ReportData);
            SaleHeaderOutputDto differentHeader = await GetHeaderDifferent(previousSaleCalculation.ReportHeader, currentSaleCalculation.ReportHeader, input);

            var (previousItems, currentItems, differentItems) = await GetData(previousSaleCalculation.ReportData, currentSaleCalculation.ReportData);
            AfterSaleDataOutputDto data=new(previousItems, currentItems, differentItems);
            FlatReportOutput<SaleHeaderOutputDto, AfterSaleDataOutputDto> result = new(_title, differentHeader, data);
            return result;
        }
        private void ValidationOffering(AfterSaleInputDto input)
        {
<<<<<<< HEAD
            x(input.CompanyServiceIds);
=======
>>>>>>> 2a14f43b7b9aae3264bf22751794a0853d384749
            if (input.PreviousData.WaterDiameterId != input.CurrentData.WaterDiameterId && !input.CompanyServiceIds.Contains(1))
            {
                throw new AfterSaleException(ExceptionLiterals.CheckCompanyService(ExceptionLiterals.ChangeWaterDiameter));
            }
            if (input.PreviousData.SiphonDiameterId != input.CurrentData.SiphonDiameterId && !input.CompanyServiceIds.Contains(24))
            {
                throw new AfterSaleException(ExceptionLiterals.CheckCompanyService(ExceptionLiterals.ChangeSiphonDiameter));
            }

            if (input.PreviousData.UsageId != input.CurrentData.UsageId && !input.CompanyServiceIds.Contains(7))
            {
                throw new AfterSaleException(ExceptionLiterals.CheckCompanyService(ExceptionLiterals.ChangeUsage));
            }

        }
        private SaleInputDto GetPreviousSaleInput(AfterSaleInputDto input)
        {
            return new SaleInputDto()
            {
                ZoneId = input.ZoneId,
                UsageId = input.PreviousData.UsageId,
                Block = input.Block,
                IsDomestic = input.PreviousData.IsDomestic,
                DiscountTypeId = input.PreviousData.DiscountTypeId,
                HasWaterBroker = input.PreviousData.HasWaterBroker,
                ContractualCapacity = input.PreviousData.ContractualCapacity,
                DomesticUnit = input.PreviousData.DomesticUnit,
                CommertialUnit = input.PreviousData.CommertialUnit,
                OtherUnit = input.PreviousData.OtherUnit,
                WaterDiameterId = input.PreviousData.WaterDiameterId,
                SiphonDiameterId = input.PreviousData.SiphonDiameterId,
            };
        }
        private SaleInputDto GetCurrentSaleInput(AfterSaleInputDto input)
        {
            return new SaleInputDto()
            {
                ZoneId = input.ZoneId,
                UsageId = input.CurrentData.UsageId,
                Block = input.Block,
                IsDomestic = input.CurrentData.IsDomestic,
                DiscountTypeId = input.CurrentData.DiscountTypeId,
                HasWaterBroker = input.CurrentData.HasWaterBroker,
                ContractualCapacity = input.CurrentData.ContractualCapacity,
                DomesticUnit = input.CurrentData.DomesticUnit,
                CommertialUnit = input.CurrentData.CommertialUnit,
                OtherUnit = input.CurrentData.OtherUnit,
                WaterDiameterId = input.CurrentData.WaterDiameterId,
                SiphonDiameterId = input.CurrentData.SiphonDiameterId,
            };
        }
        private IEnumerable<SaleDataOutputDto> GetDifferentData(IEnumerable<SaleDataOutputDto> previousSaleCalculation, IEnumerable<SaleDataOutputDto> currentSaleCalculation)
        {
            var previousData = previousSaleCalculation.ToDictionary(x => x.Id);
            var currentData = currentSaleCalculation.ToDictionary(x => x.Id);

            ICollection<SaleDataOutputDto> allData = previousData
                .Keys
                .Union(currentData.Keys)
                .Select(id => new SaleDataOutputDto
                (
                    id,
                    previousData.GetValueOrDefault(id)?.Title,
                    CalcDiff(currentData.GetValueOrDefault(id)?.Amount, previousData.GetValueOrDefault(id)?.Amount),
                    CalcDiff(currentData.GetValueOrDefault(id)?.Discount, previousData.GetValueOrDefault(id)?.Discount),
                    CalcDiff(currentData.GetValueOrDefault(id)?.FinalAmount, previousData.GetValueOrDefault(id)?.FinalAmount)
                ))
                .ToList();

            return allData.ToList();
        }
        private async Task<SaleHeaderOutputDto> GetHeaderDifferent(SaleHeaderOutputDto previousSaleHeader, SaleHeaderOutputDto currentSaleHeader, AfterSaleInputDto input)
        {
            bool hasBroker = input.CurrentData.HasWaterBroker ?? (await _equipmentBrokerAndZoneQueryService.Get(input.ZoneId)) is { Id: > 0 };
            return new SaleHeaderOutputDto()
            {
                HasBroker = hasBroker,
                CompanyAmount = currentSaleHeader.CompanyAmount - previousSaleHeader.CompanyAmount,
                CompanyDiscountAmount = currentSaleHeader.CompanyDiscountAmount - previousSaleHeader.CompanyDiscountAmount,
                CompanyFinalAmount = currentSaleHeader.CompanyFinalAmount - previousSaleHeader.CompanyFinalAmount,
                CompanyItemCount = currentSaleHeader.CompanyItemCount,

                BrokerAmount = currentSaleHeader.BrokerAmount - previousSaleHeader.BrokerAmount,
                BrokerItemCount = currentSaleHeader.BrokerItemCount,

                SumAmount = currentSaleHeader.SumAmount - previousSaleHeader.SumAmount,
                PayableAmount = currentSaleHeader.PayableAmount - previousSaleHeader.PayableAmount,
                ItemCount = currentSaleHeader.ItemCount,
            };
        }
        private async Task<(IEnumerable<SaleDataOutputDto>, IEnumerable<SaleDataOutputDto>, IEnumerable<SaleDataOutputDto>)> GetData(IEnumerable<SaleDataOutputDto> previousSaleData, IEnumerable<SaleDataOutputDto> currentSaleData)
        {
            string waterInstallationTitle = await _offeringQueryService.Get((short)OfferingEnum.WaterInstallation);
            string sewageInstallationTitle = await _offeringQueryService.Get((short)OfferingEnum.SewageInstalltion);
            string waterEquipmentTitle = await _offeringQueryService.Get((short)OfferingEnum.WaterEquipment);
            string sewageEquipmentTitle = await _offeringQueryService.Get((short)OfferingEnum.SewageEquipment);
            string waterArtile11Title = await _offeringQueryService.Get((short)OfferingEnum.WaterArticle11);
            string sewageArtile11Title = await _offeringQueryService.Get((short)OfferingEnum.SewageArticle11);

            var previousDictionary = previousSaleData.ToDictionary(x => x.Id);
            var currentDictionary = currentSaleData.ToDictionary(x => x.Id);
            var previousItems = new List<SaleDataOutputDto>
            {
                CreateOrGetZeroItem((short)OfferingEnum.WaterInstallation, waterInstallationTitle, previousDictionary),
                CreateOrGetZeroItem((short)OfferingEnum.SewageInstalltion, sewageInstallationTitle, previousDictionary),
                CreateOrGetZeroItem((short)OfferingEnum.WaterEquipment, waterEquipmentTitle, previousDictionary),
                CreateOrGetZeroItem((short)OfferingEnum.SewageEquipment, sewageEquipmentTitle, previousDictionary),
                CreateOrGetZeroItem((short)OfferingEnum.WaterArticle11, waterArtile11Title, previousDictionary),
                CreateOrGetZeroItem((short)OfferingEnum.SewageArticle11, sewageArtile11Title, previousDictionary)
            };
            var currentItems = new List<SaleDataOutputDto>
            {
                CreateOrGetZeroItem((short)OfferingEnum.WaterInstallation, waterInstallationTitle, currentDictionary),
                CreateOrGetZeroItem((short)OfferingEnum.SewageInstalltion, sewageInstallationTitle, currentDictionary),
                CreateOrGetZeroItem((short)OfferingEnum.WaterEquipment, waterEquipmentTitle, currentDictionary),
                CreateOrGetZeroItem((short)OfferingEnum.SewageEquipment, sewageEquipmentTitle, currentDictionary),
                CreateOrGetZeroItem((short)OfferingEnum.WaterArticle11, waterArtile11Title, currentDictionary),
                CreateOrGetZeroItem((short)OfferingEnum.SewageArticle11, sewageArtile11Title, currentDictionary)
            };
            var previousData = previousItems.ToDictionary(x => x.Id);
            var currentData = currentItems.ToDictionary(x => x.Id);

            ICollection<SaleDataOutputDto> differentItems = previousData
                .Keys
                .Union(currentData.Keys)
                .Select(id => new SaleDataOutputDto
                (
                    id,
                    previousData.GetValueOrDefault(id)?.Title,
                    CalcDiff(currentData.GetValueOrDefault(id)?.Amount, previousData.GetValueOrDefault(id)?.Amount),
                    CalcDiff(currentData.GetValueOrDefault(id)?.Discount, previousData.GetValueOrDefault(id)?.Discount),
                    CalcDiff(currentData.GetValueOrDefault(id)?.FinalAmount, previousData.GetValueOrDefault(id)?.FinalAmount)
                ))
                .ToList();

            SaleDataOutputDto CreateOrGetZeroItem(short id, string title, Dictionary<short, SaleDataOutputDto> data)
            {
                if (data.TryGetValue(id, out var existingItem))
                {
                    return new SaleDataOutputDto(existingItem.Id, title, existingItem.Amount, existingItem.Discount, existingItem.FinalAmount);
                }
                return new SaleDataOutputDto(id, title, 0, 0, 0);

            }


            return (previousItems, currentItems, differentItems);
        }
        long CalcDiff(long? current, long? previous) => (current ?? 0) - (previous ?? 0);

<<<<<<< HEAD
        private void x(ICollection<int> offeringIds)
        {
            var s = offeringIds.Select(s => (XEnum)s).ToList();

            var ss = s;
        }
    }
    public enum XEnum
    {
        WastewaterBranch = 3,
        MeterSeparation = 4,
        ChangeSpecifications = 5,
        ChangeUnit = 6,
        ChangeUsage = 7,
        ChangeBranchDiameter = 8,
        InstallAdditionalSiphon = 9,
        MeterRelocation = 10,
        BranchDismantling = 11,
        PropertyConsolidationAndMerger = 12,
        TankerWaterSale = 13,
        SuggestionsComplaintsCriticisms = 14,
        BillPaymentFacility = 15,
        ViewRecords = 16,
        Settlement = 17,
        BillReview = 18,
        InterimBill = 19,
        TemporaryDisconnectionAndReconnection = 20,
        MeterTest = 21,
        WaterAndWastewaterBranchTransfer = 22,
        AfterSalesService = 23,
        ChangeSiphonDiameter = 24,
        SiphonReplacement = 25,
        NotaryInquiry = 26,
        DisconnectionAndReconnection = 27,
        SiphonRelocation = 28,
        EngineeringSystem = 29,
        MeterReplacement = 30,
        ChangeContractualCapacity = 31,
        HouseholdCensus = 42,
        WaterPreparation = 45,
        WastewaterPreparation = 46,
        TariffChange = 47,
        Surveying = 67,
        OtherServices = 74,
        ChangeMeterLevel = 77
=======
>>>>>>> 2a14f43b7b9aae3264bf22751794a0853d384749
    }
}
