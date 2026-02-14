using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using Aban360.ReportPool.GatewayAdhoc.Features.ConsumersInfo;
using Aban360.ReportPool.GatewayAdhoc.Features.ConsumersInfo.Contracts;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Implementations
{
    public class AfterSaleGetHandler : IAfterSaleGetHandler
    {
        private readonly ISaleGetHandler _saleGetHandler;
        private readonly IEquipmentBrokerAndZoneQueryService _equipmentBrokerAndZoneQueryService;
        private readonly IOfferingQueryService _offeringQueryService;
        private readonly ITable3QueryService _table3QueryService;
        private readonly IZoneAddHoc _zoneAddHoc;
        private static string _title = "پس از فروش";
        private static string _article2Title = "تبصره 2";
        private static string _taxTitle = "مالیات بر ارزش افزوده";
        private static float _tax = 0.1f;
        public AfterSaleGetHandler(
            ISaleGetHandler saleGetHandler,
            IEquipmentBrokerAndZoneQueryService equipmentBrokerAndZoneQueryService,
            IOfferingQueryService offeringQueryService,
            ITable3QueryService table3QueryService,
            IZoneAddHoc zoneAddHoc)
        {
            _saleGetHandler = saleGetHandler;
            _saleGetHandler.NotNull(nameof(saleGetHandler));

            _equipmentBrokerAndZoneQueryService = equipmentBrokerAndZoneQueryService;
            _equipmentBrokerAndZoneQueryService.NotNull(nameof(equipmentBrokerAndZoneQueryService));

            _offeringQueryService = offeringQueryService;
            _offeringQueryService.NotNull(nameof(offeringQueryService));

            _table3QueryService = table3QueryService;
            _table3QueryService.NotNull(nameof(table3QueryService));

            _zoneAddHoc = zoneAddHoc;
            _zoneAddHoc.NotNull(nameof(zoneAddHoc));
        }

        public async Task<FlatReportOutput<SaleHeaderOutputDto, AfterSaleDataOutputDto>> Handle(AfterSaleInputDto input, CancellationToken cancellationToken)
        {
            //ToDo : ValidationDto
            ValidationOffering(input);

            SaleInputDto previousDataInput = GetSaleInput(input.PreviousData, input.ZoneId, input.Block, input.HasWaterArticle11, input.HasSewageArticle11);
            SaleInputDto currentDataInput = GetSaleInput(input.CurrentData, input.ZoneId, input.Block, input.HasWaterArticle11, input.HasSewageArticle11);

            ReportOutput<SaleHeaderOutputDto, SaleDataOutputDto> previousSaleCalculation = await _saleGetHandler.Handle(previousDataInput, cancellationToken);
            ReportOutput<SaleHeaderOutputDto, SaleDataOutputDto> currentSaleCalculation = await _saleGetHandler.Handle(currentDataInput, cancellationToken);

            AfterSaleDataOutputDto data = await GetData(input, previousSaleCalculation.ReportData, currentSaleCalculation.ReportData);
            SaleHeaderOutputDto header = await GetHeader(data, input);
            FlatReportOutput<SaleHeaderOutputDto, AfterSaleDataOutputDto> result = new(_title, header, data);
            return result;
        }
        private (SaleDataOutputDto, SaleDataOutputDto, SaleDataOutputDto) GetDataWithTax(IEnumerable<SaleDataOutputDto> differentData, AfterSaleInputDto input)
        {
            long taxAmount = 0;
            IEnumerable<AfterSaleCompanyServiceEnum> companyServiceEnum = GetAfterSaleCompanyServiceSelected(input.CompanyServiceIds);
            if (companyServiceEnum.Contains(AfterSaleCompanyServiceEnum.ChangeMeterDiameter))
            {
                taxAmount += differentData.Where(s => s.Id == (short)OfferingEnum.WaterInstallation).FirstOrDefault().FinalAmount;
            }
            if (companyServiceEnum.Contains(AfterSaleCompanyServiceEnum.ChangeSiphonDiameter) && input.PreviousData.SiphonDiameterId.HasValue)
            {
                taxAmount += differentData.Where(s => s.Id == (short)OfferingEnum.SewageInstalltion).FirstOrDefault().FinalAmount;
            }
            if (companyServiceEnum.Contains(AfterSaleCompanyServiceEnum.MeterRelocation))
            {
                taxAmount += differentData.Where(s => s.Id == (short)AfterSaleCompanyServiceEnum.MeterRelocation).FirstOrDefault().FinalAmount;
            }
            if (companyServiceEnum.Contains(AfterSaleCompanyServiceEnum.ChangeSpecifications))
            {
                taxAmount += differentData.Where(s => s.Id == (short)AfterSaleCompanyServiceEnum.ChangeSpecifications).FirstOrDefault().FinalAmount;
            }
            SaleDataOutputDto previousTax = new(0, _taxTitle, 0, 0, 0);
            SaleDataOutputDto currentTax = new(0, _taxTitle, 0, 0, 0);
            SaleDataOutputDto differentTax = new(0, _taxTitle, (long)(taxAmount * 0.1f), 0, (long)(taxAmount * 0.1f));

            return (previousTax, currentTax, differentTax);
        }
        private async Task<AfterSaleDataOutputDto> GetData(AfterSaleInputDto input, IEnumerable<SaleDataOutputDto> previousSaleCalculationData, IEnumerable<SaleDataOutputDto> currentSaleCalculationData)
        {
            var (previousItems, currentItems, differentItems) = await GetCompanyData(previousSaleCalculationData, currentSaleCalculationData, input);
            AfterSaleDataOutputDto companyServiceData = new(previousItems, currentItems, differentItems);
            AfterSaleDataOutputDto companyServiceWithChangeCompany = await GetAllCompanyService(input, companyServiceData);
            //11
            foreach (var item in companyServiceWithChangeCompany.DifferentValue)
            {
                if (item.FinalAmount < 0)
                {
                    if (item.Id == (short)OfferingEnum.WaterSubscription || item.Id == (short)OfferingEnum.SewageSubscription)
                    {
                        if (input.CompanyServiceIds.Contains(11))
                        {
                            item.FinalAmount = (long)(item.FinalAmount * 0.5);
                        }
                        else
                        {
                            item.FinalAmount = (long)(item.FinalAmount * 0.3);
                        }
                    }
                    else
                    {
                        item.FinalAmount = 0;
                    }
                }
            }

            return companyServiceWithChangeCompany;
        }
        private void ValidationOffering(AfterSaleInputDto input)
        {
            int[] domesticUsageWithoutContractual = { 1, 34 };
            IEnumerable<AfterSaleCompanyServiceEnum> afterSaleCompanySelected = GetAfterSaleCompanyServiceSelected(input.CompanyServiceIds);
            if (input.PreviousData.WaterDiameterId != input.CurrentData.WaterDiameterId && !afterSaleCompanySelected.Contains(AfterSaleCompanyServiceEnum.ChangeMeterDiameter))
            {
                throw new AfterSaleException(ExceptionLiterals.CheckCompanyService(ExceptionLiterals.ChangeWaterDiameter));
            }
            if (input.PreviousData.SiphonDiameterId != input.CurrentData.SiphonDiameterId && !afterSaleCompanySelected.Contains(AfterSaleCompanyServiceEnum.ChangeSiphonDiameter))
            {
                throw new AfterSaleException(ExceptionLiterals.CheckCompanyService(ExceptionLiterals.ChangeSiphonDiameter));
            }
            if (input.PreviousData.SiphonDiameterId is null && input.CurrentData.SiphonDiameterId is not null && !afterSaleCompanySelected.Contains(AfterSaleCompanyServiceEnum.WastewaterBranch))
            {
                throw new AfterSaleException(ExceptionLiterals.CheckCompanyService(ExceptionLiterals.GetSewage));
            }
            if (input.PreviousData.SiphonDiameterId is not null && input.CurrentData.SiphonDiameterId is not null
                && input.PreviousData.SiphonDiameterId != input.CurrentData.SiphonDiameterId
                && !afterSaleCompanySelected.Contains(AfterSaleCompanyServiceEnum.ChangeSiphonDiameter))
            {
                throw new AfterSaleException(ExceptionLiterals.CheckCompanyService(ExceptionLiterals.ChangeSiphonDiameter));
            }
            if (input.PreviousData.UsageId != input.CurrentData.UsageId && !afterSaleCompanySelected.Contains(AfterSaleCompanyServiceEnum.ChangeUsage))
            {
                throw new AfterSaleException(ExceptionLiterals.CheckCompanyService(ExceptionLiterals.ChangeUsage));
            }
            if (!domesticUsageWithoutContractual.Contains(input.PreviousData.UsageId) && input.CurrentData.UsageId == 3 &&
                input.PreviousData.ContractualCapacity != input.CurrentData.ContractualCapacity && !afterSaleCompanySelected.Contains(AfterSaleCompanyServiceEnum.ChangeContractualCapacity))
            {
                throw new AfterSaleException(ExceptionLiterals.CheckCompanyService(ExceptionLiterals.ChangeContractualCapacity));
            }


            //if (input.PreviousData.DiscountTypeId.HasValue && input.PreviousData.DiscountTypeId.Value > 0 &&
            //    input.CurrentData.DiscountTypeId.HasValue && input.CurrentData.DiscountTypeId.Value > 0)
            //{
            //    throw new AfterSaleException(ExceptionLiterals.InvalidTwoDiscount);
            //}   TODO
            if (input.PreviousData.DiscountTypeId.HasValue && input.PreviousData.DiscountTypeId.Value > 0 &&
                !input.PreviousData.DiscountCount.HasValue)
            {
                throw new AfterSaleException(ExceptionLiterals.InvalidDiscountCount);
            }
            if (input.CurrentData.DiscountTypeId.HasValue && input.CurrentData.DiscountTypeId.Value > 0 &&
                !input.CurrentData.DiscountCount.HasValue)
            {
                throw new AfterSaleException(ExceptionLiterals.InvalidDiscountCount);
            }
        }
        private async Task<AfterSaleDataOutputDto> GetAllCompanyService(AfterSaleInputDto input, AfterSaleDataOutputDto companyServiceData)
        {
            Table3InputDto table3Input = new(input.ZoneId, input.CurrentData.UsageId, input.CompanyServiceIds);
            IEnumerable<Table3GetDto> table3 = await _table3QueryService.Get(table3Input);
            ICollection<SaleDataOutputDto> previousItems = new List<SaleDataOutputDto>();
            ICollection<SaleDataOutputDto> currentItems = new List<SaleDataOutputDto>();
            ICollection<SaleDataOutputDto> differentItems = new List<SaleDataOutputDto>();
            if (table3 is not null && table3.Count() > 0)
            {
                foreach (var item in table3)
                {
                    previousItems.Add(new SaleDataOutputDto((short)item.CompanyServiceId, item.CompanyServiceTitle, 0, 0, 0));
                    currentItems.Add(new SaleDataOutputDto((short)item.CompanyServiceId, item.CompanyServiceTitle, 0, 0, 0));
                    differentItems.Add(new SaleDataOutputDto((short)item.CompanyServiceId, item.CompanyServiceTitle, item.Price, 0, item.Price));
                }
            }
            var (previousTax, currentTax, differentTax) = GetDataWithTax(companyServiceData.DifferentValue.Concat(differentItems).ToList(), input);
            previousItems.Add(previousTax);
            currentItems.Add(currentTax);
            differentItems.Add(differentTax);

            if (!HasSiphon(input.CurrentData.SiphonDiameterId))
            {
                var (previousArticle11, currentArticle11, differentArticle11) = await GetSewageArticle2(input.ZoneId, companyServiceData.DifferentValue.Where(s => s.Id == (short)OfferingEnum.WaterSubscription).FirstOrDefault().FinalAmount);
                previousItems.Add(previousArticle11);
                currentItems.Add(currentArticle11);
                differentItems.Add(differentArticle11);
            }



            AfterSaleDataOutputDto companyServiceResult = new(companyServiceData.PreviousValue.Concat(previousItems), companyServiceData.CurrentValue.Concat(currentItems), companyServiceData.DifferentValue.Concat(differentItems));

            return companyServiceResult;
        }
        private async Task<(SaleDataOutputDto, SaleDataOutputDto, SaleDataOutputDto)> GetSewageArticle2(int zoneId, long amount)
        {
            bool hasArticle11 = await _zoneAddHoc.GetArticle2(zoneId);
            long article11Amount = hasArticle11 ? (long)(amount * 0.1f) : 0;

            SaleDataOutputDto previousArticle11 = new(79, _article2Title, 0, 0, 0);
            SaleDataOutputDto currentArticle11 = new(79, _article2Title, 0, 0, 0);
            SaleDataOutputDto differentArticle11 = new(79, _article2Title, article11Amount, 0, article11Amount);

            return (previousArticle11, currentArticle11, differentArticle11);
        }
        private SaleInputDto GetSaleInput(AfterSaleItemsInputDto input, int zoneId, string? block, bool hasWaterArticle11, bool hasSewageArticle11)
        {
            return new SaleInputDto()
            {
                ZoneId = zoneId,
                UsageId = input.UsageId,
                Block = block,
                //IsDomestic = input.IsDomestic,
                DiscountTypeId = input.DiscountTypeId,
                DiscountCount = input.DiscountCount,
                HasWaterBroker = input.HasWaterBroker,
                ContractualCapacity = input.ContractualCapacity,
                DomesticUnit = input.DomesticUnit,
                CommertialUnit = input.CommertialUnit,
                OtherUnit = input.OtherUnit,
                WaterDiameterId = input.WaterDiameterId,
                SiphonDiameterId = input.SiphonDiameterId,
                IsWaterDiscount = input.IsWaterDiscount,
                IsSewageDiscount = input.IsSewageDiscount,
                HasWaterArticle11 = hasWaterArticle11,
                HasSewageArticle11 = hasSewageArticle11
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
        private async Task<SaleHeaderOutputDto> GetHeader(AfterSaleDataOutputDto data, AfterSaleInputDto input)
        {
            bool hasBroker = input.CurrentData.HasWaterBroker ?? (await _equipmentBrokerAndZoneQueryService.Get(input.ZoneId)) is { Id: > 0 };
            short[] brokerOffering = [(short)OfferingEnum.WaterEquipment];

            IEnumerable<SaleDataOutputDto> differentData = data.DifferentValue;
            IEnumerable<SaleDataOutputDto>? brokerValues = hasBroker ? differentData.Where(s => brokerOffering.Contains(s.Id)) : null;
            IEnumerable<SaleDataOutputDto>? companyValues = hasBroker ? differentData.Where(s => !brokerOffering.Contains(s.Id)) : differentData;

            return new SaleHeaderOutputDto()
            {
                HasBroker = hasBroker,

                BrokerAmount = hasBroker ? brokerValues.Sum(s => s.Amount) : 0,
                BrokerItemCount = hasBroker ? brokerValues.Count() : 0,

                CompanyAmount = hasBroker ? companyValues.Sum(s => s.Amount) : differentData.Sum(s => s.Amount),
                CompanyDiscountAmount = hasBroker ? companyValues.Sum(s => s.Discount) : differentData.Sum(s => s.Discount),
                CompanyFinalAmount = hasBroker ? companyValues.Sum(s => s.FinalAmount) : differentData.Sum(s => s.FinalAmount),
                CompanyItemCount = hasBroker ? companyValues.Count() : differentData.Count(),

                SumAmount = differentData.Sum(s => s.Amount),
                PayableAmount = differentData.Sum(s => s.FinalAmount),
                ItemCount = differentData.Count(),
            };
        }
        private async Task<(IEnumerable<SaleDataOutputDto>, IEnumerable<SaleDataOutputDto>, IEnumerable<SaleDataOutputDto>)> GetCompanyData(IEnumerable<SaleDataOutputDto> previousSaleData, IEnumerable<SaleDataOutputDto> currentSaleData, AfterSaleInputDto input)
        {
            string waterSubscriptionTitle = await _offeringQueryService.Get((short)OfferingEnum.WaterSubscription);
            string sewageSubscriptionTitle = await _offeringQueryService.Get((short)OfferingEnum.SewageSubscription);
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
                CreateOrGetZeroItem((short)OfferingEnum.WaterSubscription, waterSubscriptionTitle, previousDictionary),
                CreateOrGetZeroItem((short)OfferingEnum.SewageSubscription, sewageSubscriptionTitle, previousDictionary),
                CreateOrGetZeroItem((short)OfferingEnum.WaterInstallation, waterInstallationTitle, previousDictionary),
                CreateOrGetZeroItem((short)OfferingEnum.SewageInstalltion, sewageInstallationTitle, previousDictionary),
                CreateOrGetZeroItem((short)OfferingEnum.WaterEquipment, waterEquipmentTitle, previousDictionary),
                CreateOrGetZeroItem((short)OfferingEnum.SewageEquipment, sewageEquipmentTitle, previousDictionary),
                CreateOrGetZeroItem((short)OfferingEnum.WaterArticle11, waterArtile11Title, previousDictionary),
                CreateOrGetZeroItem((short)OfferingEnum.SewageArticle11, sewageArtile11Title, previousDictionary)
            };
            var currentItems = new List<SaleDataOutputDto>
            {
                CreateOrGetZeroItem((short)OfferingEnum.WaterSubscription, waterSubscriptionTitle, currentDictionary),
                CreateOrGetZeroItem((short)OfferingEnum.SewageSubscription, sewageSubscriptionTitle, currentDictionary),
                CreateOrGetZeroItem((short)OfferingEnum.WaterInstallation, waterInstallationTitle, currentDictionary),
                CreateOrGetZeroItem((short)OfferingEnum.SewageInstalltion, sewageInstallationTitle, currentDictionary),
                CreateOrGetZeroItem((short)OfferingEnum.WaterEquipment, waterEquipmentTitle, currentDictionary),
                CreateOrGetZeroItem((short)OfferingEnum.SewageEquipment, sewageEquipmentTitle, currentDictionary),
                CreateOrGetZeroItem((short)OfferingEnum.WaterArticle11, waterArtile11Title, currentDictionary),
                CreateOrGetZeroItem((short)OfferingEnum.SewageArticle11, sewageArtile11Title, currentDictionary),
            };
            var previousData = previousItems.ToDictionary(x => x.Id);
            var currentData = currentItems.ToDictionary(x => x.Id);

            //short[] dontCalcDifferent = { (short)OfferingEnum.WaterEquipment, (short)OfferingEnum.SewageEquipment, (short)OfferingEnum.WaterInstallation, (short)OfferingEnum.SewageInstalltion };
            ICollection<SaleDataOutputDto> differentItems = previousData
                .Keys
                .Union(currentData.Keys)
                .Select(id =>
                {
                    bool hasWaterDiameterChange = HasDiamterChange(input.PreviousData.WaterDiameterId, input.CurrentData.WaterDiameterId);
                    bool hasSewageDiameterChange = HasDiamterChange(input.PreviousData.SiphonDiameterId, input.CurrentData.SiphonDiameterId);

                    bool hasChange = false;
                    if (id == (short)OfferingEnum.WaterEquipment || id == (short)OfferingEnum.WaterInstallation)
                        hasChange = hasWaterDiameterChange;
                    else if (id == (short)OfferingEnum.SewageEquipment || id == (short)OfferingEnum.SewageInstalltion)
                        hasChange = hasSewageDiameterChange;


                    long amount = CalcDiff(currentData.GetValueOrDefault(id)?.Amount, previousData.GetValueOrDefault(id)?.Amount);
                    long discount = CalcDiff(currentData.GetValueOrDefault(id)?.Discount, previousData.GetValueOrDefault(id)?.Discount);
                    long finalAmount = CalcDiff(currentData.GetValueOrDefault(id)?.FinalAmount, previousData.GetValueOrDefault(id)?.FinalAmount);

                    return new SaleDataOutputDto
                    (
                        id,
                        previousData.GetValueOrDefault(id)?.Title,
                        hasChange ? currentData.GetValueOrDefault(id)?.Amount : amount,
                        hasChange ? currentData.GetValueOrDefault(id)?.Discount : discount,
                        hasChange ? currentData.GetValueOrDefault(id)?.FinalAmount : finalAmount
                    );
                })
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

        private IEnumerable<AfterSaleCompanyServiceEnum> GetAfterSaleCompanyServiceSelected(ICollection<int> offeringIds)
        {
            return offeringIds
                .Select(s => (AfterSaleCompanyServiceEnum)s)
                .ToList();
        }
        private bool HasDiamterChange(int? previousDiameter, int? currentDiameter)
        {
            return previousDiameter.HasValue && currentDiameter.HasValue && previousDiameter.Value == currentDiameter.Value ?
                false :
                true;
        }
        private bool HasSiphon(int? siphonDiameterId) => siphonDiameterId != null && siphonDiameterId > 0 ? true : false;
    }
    public enum AfterSaleCompanyServiceEnum
    {
        WastewaterBranch = 3,
        MeterSeparation = 4,
        ChangeSpecifications = 5,
        ChangeUnit = 6,
        ChangeUsage = 7,
        ChangeMeterDiameter = 8,
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
    }

}

