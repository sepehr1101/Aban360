using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using Aban360.ReportPool.GatewayAdhoc.Features.ConsumersInfo.Contracts;
using DNTPersianUtils.Core;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Implementations
{
    internal sealed class SaleGetHandler : ISaleGetHandler
    {
        private readonly IInstallationAndEquipmentQueryService _installationAndEquipmentService;
        private readonly IArticle11QueryService _article11QueryService;
        private readonly IEquipmentBrokerAndZoneQueryService _equipmentBrokerAndZoneQueryService;
        private readonly IOfferingQueryService _offeringQueryService;
        private readonly IAdjustmentFactorQueryService _adjustmentFactorQueryService;
        private readonly IZoneAddHoc _zoneAddHoc;
        private readonly IValidator<SaleInputDto> _validator;
        private readonly int[] _domesticUsage = { 1, 34 };
        private static string _reportTitle = "فروش انشعاب";
        private static string _article2Title = "تبصره 2";
        private static int _nonDomesticAjustmentFactor = 285000;
        private static int _nonDomesticFixed = 1140000;
        private static float _domesticSewageMultiplier = 0.7f;
        private static float _nonDomesticSewageMultiplier = 1f;
        public SaleGetHandler(
            IInstallationAndEquipmentQueryService installationAndEquipmentService,
            IArticle11QueryService article11QueryService,
            IEquipmentBrokerAndZoneQueryService equipmentBrokerAndZoneQueryService,
            IOfferingQueryService offeringQueryService,
            IAdjustmentFactorQueryService adjustmentFactorQueryService,
            ICustomerInfoService customerInfoService,
            IZoneAddHoc zoneAddHoc,
            IValidator<SaleInputDto> validator)
        {
            _installationAndEquipmentService = installationAndEquipmentService;
            _installationAndEquipmentService.NotNull(nameof(installationAndEquipmentService));

            _article11QueryService = article11QueryService;
            _article11QueryService.NotNull(nameof(article11QueryService));

            _equipmentBrokerAndZoneQueryService = equipmentBrokerAndZoneQueryService;
            _equipmentBrokerAndZoneQueryService.NotNull(nameof(equipmentBrokerAndZoneQueryService));

            _offeringQueryService = offeringQueryService;
            _offeringQueryService.NotNull(nameof(offeringQueryService));

            _adjustmentFactorQueryService = adjustmentFactorQueryService;
            _adjustmentFactorQueryService.NotNull(nameof(adjustmentFactorQueryService));

            _zoneAddHoc = zoneAddHoc;
            _zoneAddHoc.NotNull(nameof(zoneAddHoc));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<SaleHeaderOutputDto, SaleDataOutputDto>> Handle(SaleInputDto inputDto, CancellationToken cancellationToken)
        {
            await Validation(inputDto, cancellationToken);
            bool hasBroker = inputDto.HasWaterBroker ?? (await _equipmentBrokerAndZoneQueryService.Get(inputDto.ZoneId)) is { Id: > 0 };

            IEnumerable<SaleDataOutputDto> data = await GetData(inputDto);
            SaleHeaderOutputDto header = CalcSaleHeader(data, hasBroker);
            ReportOutput<SaleHeaderOutputDto, SaleDataOutputDto> result = new(_reportTitle, header, data);

            return result;
        }
        public async Task<ReportOutput<SaleHeaderReportOutputDto, SaleDataOutputDto>> ReportHandle(SaleInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<SaleHeaderOutputDto, SaleDataOutputDto> data = await Handle(inputDto, cancellationToken);
            SaleHeaderReportOutputDto reportHeader = new SaleHeaderReportOutputDto()
            {
                HasBroker = data.ReportHeader.HasBroker,
                BrokerAmount = data.ReportHeader.BrokerAmount,
                BrokerItemCount = data.ReportHeader.BrokerItemCount,
                CompanyAmount = data.ReportHeader.CompanyAmount,
                CompanyDiscountAmount = data.ReportHeader.CompanyDiscountAmount,
                CompanyFinalAmount = data.ReportHeader.CompanyFinalAmount,
                CompanyItemCount = data.ReportHeader.CompanyItemCount,
                SumAmount = data.ReportHeader.SumAmount,
                PayableAmount = data.ReportHeader.PayableAmount,
                ItemCount = data.ReportHeader.ItemCount
            };

            return new ReportOutput<SaleHeaderReportOutputDto, SaleDataOutputDto>(data.Title, reportHeader, data.ReportData);
        }
        private SaleHeaderOutputDto CalcSaleHeader(IEnumerable<SaleDataOutputDto> salesData, bool hasBroker)
        {
            short[] brokerOffering = [(short)OfferingEnum.WaterEquipment];

            IEnumerable<SaleDataOutputDto>? brokerValues = hasBroker ? salesData.Where(s => brokerOffering.Contains(s.Id)) : null;
            IEnumerable<SaleDataOutputDto>? companyValues = hasBroker ? salesData.Where(s => !brokerOffering.Contains(s.Id)) : salesData;

            SaleHeaderOutputDto headerOutput = new SaleHeaderOutputDto()
            {
                HasBroker = hasBroker,

                BrokerAmount = hasBroker ? brokerValues.Sum(s => s.Amount) : 0,
                BrokerItemCount = hasBroker ? brokerValues.Count() : 0,

                CompanyAmount = hasBroker ? companyValues.Sum(s => s.Amount) : salesData.Sum(s => s.Amount),
                CompanyDiscountAmount = hasBroker ? companyValues.Sum(s => s.Discount) : salesData.Sum(s => s.Discount),
                CompanyFinalAmount = hasBroker ? companyValues.Sum(s => s.FinalAmount) : salesData.Sum(s => s.FinalAmount),
                CompanyItemCount = hasBroker ? companyValues.Count() : salesData.Count(),

                SumAmount = salesData.Sum(s => s.Amount),
                PayableAmount = salesData.Sum(s => s.FinalAmount),
                ItemCount = salesData.Count(),
            };

            return headerOutput;
        }
        private async Task<IEnumerable<SaleDataOutputDto>> GetData(SaleInputDto inputDto)
        {
            IEnumerable<SaleDataOutputDto> salesAmountData = await GetOffering(inputDto);
            IEnumerable<SaleDataOutputDto> salesData = GetDiscount(inputDto, salesAmountData);

            return salesData;
        }
        private async Task<IEnumerable<SaleDataOutputDto>> GetOffering(SaleInputDto inputDto)
        {
            IEnumerable<SaleDataOutputDto> installationAndEquipment = await GetInstallationAndEquipment(inputDto);
            IEnumerable<SaleDataOutputDto> article11Data = await GetArticle11(inputDto);
            IEnumerable<SaleDataOutputDto> ajustmentFactor = await GetSubscription(inputDto);

            return installationAndEquipment.Concat(article11Data).Concat(ajustmentFactor);
        }
        private async Task<ICollection<SaleDataOutputDto>> GetInstallationAndEquipment(SaleInputDto inputDto)
        {
            ICollection<SaleDataOutputDto> waterInstallationAndEquipment = await GetInstallationAndEquipmentSaleData(true, inputDto.WaterDiameterId);

            if (HasSiphon(inputDto))
            {
                ICollection<SaleDataOutputDto> sewageInstallationAndEquipment = await GetInstallationAndEquipmentSaleData(false, inputDto.SiphonDiameterId);
                return waterInstallationAndEquipment.Concat(sewageInstallationAndEquipment).ToList();
            }

            return waterInstallationAndEquipment;
        }
        private async Task<IEnumerable<SaleDataOutputDto>> GetArticle11(SaleInputDto inputDto)
        {
            if (IsVillage(inputDto.ZoneId))
                Array.Empty<SaleDataOutputDto>();

            int usageMultiplier = GetUsageMultiplierForArticle11(inputDto);
            Article11OutputDto article11Data = await GetArticle11Data(inputDto);

            SaleDataOutputDto waterArticle11 = await GetSaleData(OfferingEnum.WaterArticle11, inputDto.HasWaterArticle11 ? GetWaterAmount() : 0, null);
            if (HasSiphon(inputDto))
            {
                SaleDataOutputDto sewageArticle11 = await GetSaleData(OfferingEnum.SewageArticle11, inputDto.HasSewageArticle11 ? GetSewageAmount() : 0, null);
                return new[] { waterArticle11, sewageArticle11 };
            }

            return new[] { waterArticle11 };

            long GetWaterAmount()
            {
                return (inputDto.IsDomestic ? article11Data.DomesticWaterAmount : article11Data.NonDomesticWaterAmount) * usageMultiplier;
            }
            long? GetSewageAmount()
            {
                return (inputDto.IsDomestic ? article11Data.DomesticSewageAmount : article11Data.NonDomesticSewageAmount) * usageMultiplier;
            }
        }
        private async Task<ICollection<SaleDataOutputDto>> GetSubscription(SaleInputDto inputDto)
        {
            AdjustmentFactorGetDto adjustmentfactor = await _adjustmentFactorQueryService.Get(inputDto.ZoneId);
            long domesticAdjustmentFactor = adjustmentfactor.Price * inputDto.DomesticUnit;
            long constNonDomesticAdjustmentFactor = (long)(adjustmentfactor.AdjustmentFactor * _nonDomesticAjustmentFactor * inputDto.ContractualCapacity);
            long nonDomesticSubscription = constNonDomesticAdjustmentFactor + (_nonDomesticFixed * inputDto.CommertialUnit);

            if (HasSiphon(inputDto))
            {
                if (inputDto.IsDomestic)
                {
                    SaleDataOutputDto waterSubscription = await GetSaleData(OfferingEnum.WaterSubscription, domesticAdjustmentFactor, null);
                    SaleDataOutputDto sewageSubscription = await GetSaleData(OfferingEnum.SewageSubscription, (long)(domesticAdjustmentFactor * _domesticSewageMultiplier), null);

                    return new[] { waterSubscription, sewageSubscription };
                }
                else
                {
                    SaleDataOutputDto waterSubscription = await GetSaleData(OfferingEnum.WaterSubscription, nonDomesticSubscription, null);
                    SaleDataOutputDto sewageSubscription = await GetSaleData(OfferingEnum.SewageSubscription, (long)(nonDomesticSubscription * _nonDomesticSewageMultiplier), null);

                    return new[] { waterSubscription, sewageSubscription };
                }
            }
            else
            {
                SaleDataOutputDto waterSubscription = inputDto.IsDomestic ? await GetSaleData(OfferingEnum.WaterSubscription, domesticAdjustmentFactor, null) : await GetSaleData(OfferingEnum.WaterSubscription, (long)(nonDomesticSubscription), null);
                SaleDataOutputDto article2 = await GetSewageArticle2(inputDto.ZoneId, waterSubscription.FinalAmount);
                return new[] { waterSubscription, article2 };
            }
        }
        private async Task<ICollection<SaleDataOutputDto>> GetInstallationAndEquipmentSaleData(bool isWater, short? meterDiameterId)
        {
            var installationAndEquipment = new InstallationAndEquipmentGetDto(isWater, meterDiameterId, DateTime.Now.ToShortPersianDateString());
            InstallationAndEquipmentOutputDto installtionAndEquipmentData = await _installationAndEquipmentService.Get(installationAndEquipment);
            SaleDataOutputDto installation = await GetSaleData(isWater ? OfferingEnum.WaterInstallation : OfferingEnum.SewageInstalltion, installtionAndEquipmentData.InstallationAmount, null);
            SaleDataOutputDto equipment = await GetSaleData(isWater ? OfferingEnum.WaterEquipment : OfferingEnum.SewageEquipment, installtionAndEquipmentData.EquipmentAmount, null);

            return new List<SaleDataOutputDto> { installation, equipment };
        }
        private async Task<SaleDataOutputDto> GetSaleData(OfferingEnum offeringEnum, long? amount, long? discount)
        {
            string title = await _offeringQueryService.Get((short)offeringEnum);
            SaleDataOutputDto saleData = new((short)offeringEnum, title, amount, discount, amount);

            return saleData;
        }
        private IEnumerable<SaleDataOutputDto> GetDiscount(SaleInputDto inputDto, IEnumerable<SaleDataOutputDto> salesData)
        {
            Dictionary<int, float> discountPercentList = new Dictionary<int, float>()
            {
                { 1 , 1f },
                { 2 , 1f },
                { 4 , 1f },
                { 5 , 1f },
                { 7 , 1f },
                { 14, 1f },
                { 16, 1f },
                { 15, 0.7f },
                { 10, 0.5f }
            };
            if (!inputDto.DiscountTypeId.HasValue || inputDto.DiscountTypeId <= 0 || !IsDomestic(inputDto.UsageId))
            {
                return salesData;
            }
            if (!discountPercentList.TryGetValue(inputDto.DiscountTypeId.Value, out var discountPercent))
            {
                return salesData;
            }

            return CalcFinalDiscount(salesData, discountPercent, inputDto);
        }
        private IEnumerable<SaleDataOutputDto> CalcFinalDiscount(IEnumerable<SaleDataOutputDto> salesData, float discountPercent, SaleInputDto inputDto)
        {
            short[] waterDiscountOffering = [(short)OfferingEnum.WaterArticle11, (short)OfferingEnum.WaterSubscription];
            short[] sewageDiscountOffering = [(short)OfferingEnum.SewageArticle11, (short)OfferingEnum.SewageSubscription];

            foreach (var item in salesData)
            {
                if (waterDiscountOffering.Contains(item.Id) && inputDto.IsWaterDiscount)
                {
                    long discountPerUnit = item.Amount / inputDto.DomesticUnit;
                    item.Discount = (long)(discountPerUnit * inputDto.DiscountCount.Value * discountPercent);
                    item.FinalAmount = item.Amount - (item.Discount ?? 0);
                }
                if (sewageDiscountOffering.Contains(item.Id) && inputDto.IsSewageDiscount)
                {
                    long discountPerUnit = item.Amount / inputDto.DomesticUnit;
                    item.Discount = (long)(discountPerUnit * inputDto.DiscountCount.Value * discountPercent);
                    item.FinalAmount = item.Amount - (item.Discount ?? 0);
                }
            }

            return salesData;
        }
        private async Task<SaleDataOutputDto> GetSewageArticle2(int zoneId, long amount)
        {
            bool hasArticle11 = await _zoneAddHoc.GetArticle2(zoneId);
            long article2Amount = hasArticle11 ? (long)(amount * 0.1f) : 0;

            SaleDataOutputDto article2 = new(79, _article2Title, article2Amount, 0, article2Amount);

            return article2;
        }
        private async Task Validation(SaleInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            if (!IsVillage(inputDto.ZoneId))
            {
                if (!await _article11QueryService.ZoneValidation(inputDto.ZoneId))
                {
                    throw new SaleException(ExceptionLiterals.InvalidZoneId);
                }
                if (!await _article11QueryService.ZoneWithBlockValidation(inputDto.ZoneId, inputDto.Block))
                {
                    throw new SaleException(ExceptionLiterals.InvalicZoneIdWithBlock);
                }
            }
            if (_domesticUsage.Contains(inputDto.UsageId) && inputDto.DomesticUnit == 0)
            {
                throw new SaleException(ExceptionLiterals.InvalidDomesticUnit);
            }
        }

        private async Task<Article11OutputDto> GetArticle11Data(SaleInputDto inputDto)
        {
            var article11Dto = new Article11GetDto(inputDto.ZoneId, inputDto.Block, DateTime.Now.ToShortPersianDateString());
            return await _article11QueryService.Get(article11Dto);
        }
        private int GetUsageMultiplierForArticle11(SaleInputDto inputDto)
        {
            int usageMultiplier = inputDto.IsDomestic ? inputDto.DomesticUnit : inputDto.ContractualCapacity;
            return inputDto.UsageId == 3 ? inputDto.ContractualCapacity * inputDto.DomesticUnit : usageMultiplier;
        }

        private bool HasSiphon(SaleInputDto input) => input.SiphonDiameterId != null && input.SiphonDiameterId > 0 ? true : false;
        private bool IsDomestic(int usageId)
        {
            int[] domestic = { 1, 3 };
            return domestic.Contains(usageId);
        }
        private bool IsVillage(int zoneId) => zoneId > 140000;
    }
}
