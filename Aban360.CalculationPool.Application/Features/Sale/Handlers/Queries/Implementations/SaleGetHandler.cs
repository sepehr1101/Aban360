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
        private readonly IValidator<SaleInputDto> _validator;
        private static string title = "فروش انشعاب";
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

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<SaleHeaderOutputDto, SaleDataOutputDto>> Handle(SaleInputDto inputDto, CancellationToken cancellationToken)
        {
            await Validation(inputDto, cancellationToken);
            bool hasBroker = inputDto.HasWaterBroker ?? (await _equipmentBrokerAndZoneQueryService.Get(inputDto.ZoneId)) is { Id: > 0 };

            IEnumerable<SaleDataOutputDto> salesAmountData = await CalcOfferingAmount(inputDto);
            IEnumerable<SaleDataOutputDto> salesData = CalcDiscount(inputDto, salesAmountData);

            return CalcSaleHeader(salesData, hasBroker);
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
        private async Task Validation(SaleInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            if (inputDto.ZoneId < 140000)
            {
                if (!await _article11QueryService.ZoneWithBlockValidation(inputDto.ZoneId, inputDto.Block))
                {
                    throw new SaleException(ExceptionLiterals.InvalicZoneIdWithBlock);
                }

                if (!await _article11QueryService.ZoneValidation(inputDto.ZoneId))
                {
                    throw new SaleException(ExceptionLiterals.InvalicZoneId);
                }
            }
        }
        private ReportOutput<SaleHeaderOutputDto, SaleDataOutputDto> CalcSaleHeader(IEnumerable<SaleDataOutputDto> salesData, bool hasBroker)
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

            ReportOutput<SaleHeaderOutputDto, SaleDataOutputDto> result = new(title, headerOutput, salesData);
            return result;
        }
        private async Task<IEnumerable<SaleDataOutputDto>> CalcOfferingAmount(SaleInputDto inputDto)
        {
            IEnumerable<SaleDataOutputDto> installationAndEquipment = await GetInstallationAndEquipment(inputDto);
            IEnumerable<SaleDataOutputDto> article11Data = inputDto.ZoneId < 140000 ? await GetArticle11(inputDto) : Array.Empty<SaleDataOutputDto>();
            IEnumerable<SaleDataOutputDto> ajustmentFactor = await CalcSubscription(inputDto);

            return installationAndEquipment.Concat(article11Data).Concat(ajustmentFactor);
        }
        private async Task<ICollection<SaleDataOutputDto>> CalcSubscription(SaleInputDto inputDto)
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
                return inputDto.IsDomestic ?
                    new[] { await GetSaleData(OfferingEnum.WaterSubscription, domesticAdjustmentFactor, null) } :
                    new[] { await GetSaleData(OfferingEnum.WaterSubscription, (long)(nonDomesticSubscription), null) };
            }
        }
        private async Task<IEnumerable<SaleDataOutputDto>> GetArticle11(SaleInputDto inputDto)
        {
            int usageMultiplier = inputDto.IsDomestic ? inputDto.DomesticUnit : inputDto.ContractualCapacity;
            usageMultiplier = inputDto.UsageId == 3 ? inputDto.ContractualCapacity * inputDto.DomesticUnit : usageMultiplier;

            var article11 = new Article11GetDto(inputDto.ZoneId, inputDto.Block, DateTime.Now.ToShortPersianDateString());
            Article11OutputDto article11Data = await _article11QueryService.Get(article11);
            SaleDataOutputDto waterArticle11 = await GetSaleData(OfferingEnum.WaterArticle11, (inputDto.IsDomestic ? article11Data.DomesticWaterAmount : article11Data.NonDomesticWaterAmount) * usageMultiplier, null);

            if (HasSiphon(inputDto))
            {
                SaleDataOutputDto sewageArticle11 = await GetSaleData(OfferingEnum.SewageArticle11, (inputDto.IsDomestic ? article11Data.DomesticSewageAmount : article11Data.NonDomesticSewageAmount) * usageMultiplier, null);
                return new[] { waterArticle11, sewageArticle11 };
            }

            return new[] { waterArticle11 };
        }
        private async Task<ICollection<SaleDataOutputDto>> GetInstallationAndEquipment(SaleInputDto inputDto)
        {
            ICollection<SaleDataOutputDto> waterInstallationAndEquipment = await GetInstallationAndEquipment(true, inputDto.WaterDiameterId);

            if (HasSiphon(inputDto))
            {
                ICollection<SaleDataOutputDto> sewageInstallationAndEquipment = await GetInstallationAndEquipment(false, inputDto.SiphonDiameterId);
                return waterInstallationAndEquipment.Concat(sewageInstallationAndEquipment).ToList();
            }

            return waterInstallationAndEquipment;
        }
        private async Task<ICollection<SaleDataOutputDto>> GetInstallationAndEquipment(bool isWater, short? meterDiameterId)
        {
            var installationAndEquipment = new InstallationAndEquipmentGetDto(isWater, meterDiameterId, DateTime.Now.ToShortPersianDateString());
            InstallationAndEquipmentOutputDto installtionAndEquipmentData = await _installationAndEquipmentService.Get(installationAndEquipment);
            SaleDataOutputDto installation = await GetSaleData(isWater ? OfferingEnum.WaterInstallation : OfferingEnum.SewageInstalltion, installtionAndEquipmentData.InstallationAmount, null);
            SaleDataOutputDto equipment = await GetSaleData(isWater ? OfferingEnum.WaterEquipment : OfferingEnum.SewageEquipment, installtionAndEquipmentData.EquipmentAmount, null);

            return new List<SaleDataOutputDto> { installation, equipment };
        }
        private bool HasSiphon(SaleInputDto input)
        {
            return input.SiphonDiameterId != null && input.SiphonDiameterId > 0 ? true : false;
        }
        private async Task<SaleDataOutputDto> GetSaleData(OfferingEnum offeringEnum, long? amount, long? discount)
        {
            string title = await _offeringQueryService.Get((short)offeringEnum);
            SaleDataOutputDto saleData = new((short)offeringEnum, title, amount, discount, amount);

            return saleData;
        }
        private IEnumerable<SaleDataOutputDto> CalcDiscount(SaleInputDto inputDto, IEnumerable<SaleDataOutputDto> salesData)
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
            short[] discountOffering = [(short)OfferingEnum.WaterArticle11, (short)OfferingEnum.SewageArticle11, (short)OfferingEnum.WaterSubscription, (short)OfferingEnum.SewageSubscription];

            foreach (var item in salesData)
            {
                if (discountOffering.Contains(item.Id))
                {
                    long discountPerUnit = item.Amount / inputDto.DomesticUnit;
                    item.Discount = discountPerUnit * inputDto.DiscountCount.Value;
                    item.FinalAmount = item.Amount - (item.Discount ?? 0);
                }
            }

            return salesData;
        }
        private bool IsDomestic(int usageId)
        {
            int[] domestic = { 1, 3 };
            return domestic.Contains(usageId);
        }
    }
}
