using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
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
        private readonly IValidator<SaleInputDto> _validator;
        private static string title = "فروش انشعاب";
        public SaleGetHandler(
            IInstallationAndEquipmentQueryService installationAndEquipmentService,
            IArticle11QueryService article11QueryService,
            IEquipmentBrokerAndZoneQueryService equipmentBrokerAndZoneQueryService,
            IOfferingQueryService offeringQueryService,
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

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<ReportOutput<SaleHeaderOutputDto, SaleDataOutputDto>> Handle(SaleInputDto inputDto, CancellationToken cancellationToken)
        {
            await Validation(inputDto, cancellationToken);

            IEnumerable<SaleDataOutputDto> salesAmountData = await CalcOfferingAmount(inputDto);
            IEnumerable<SaleDataOutputDto> salesData = CalcDiscount(inputDto, salesAmountData);

            bool hasBroker = inputDto.HasWaterBroker ?? (await _equipmentBrokerAndZoneQueryService.Get(inputDto.ZoneId)) is { Id: > 0 };

            ReportOutput<SaleHeaderOutputDto, SaleDataOutputDto> finalSale = CalcSaleHeader(salesData, hasBroker);

            return finalSale;
        }

        private async Task Validation(SaleInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            bool hasRecord = await _article11QueryService.ZoneWithBlockValidation(inputDto.ZoneId, inputDto.Block);
            if (!hasRecord)
            {
                throw new SaleException(ExceptionLiterals.InvalicZoneIdWithBlock);
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
            List<SaleDataOutputDto> salesData = new List<SaleDataOutputDto>();

            ICollection<SaleDataOutputDto> waterInstallationAndEquipment = await GetInstallationAndEquipment(true, inputDto.WaterDiameterId);
            salesData.AddRange(waterInstallationAndEquipment);

            var article11 = new Article11GetDto(inputDto.ZoneId, inputDto.Block, DateTime.Now.ToShortPersianDateString());
            Article11OutputDto article11Data = await _article11QueryService.Get(article11);
            SaleDataOutputDto waterArticle11 = await GetSaleData(OfferingEnum.WaterArticle11, inputDto.IsDomestic ? article11Data.DomesticWaterAmount : article11Data.NonDomesticWaterAmount, null);
            salesData.Add(waterArticle11);

            if (HasSiphon(inputDto))
            {
                ICollection<SaleDataOutputDto> sewageInstallationAndEquipment = await GetInstallationAndEquipment(false, inputDto.SiphonDiameterId);
                salesData.AddRange(sewageInstallationAndEquipment);

                SaleDataOutputDto sewageArticle11 = await GetSaleData(OfferingEnum.SewageArticle11, inputDto.IsDomestic ? article11Data.DomesticSewageAmount : article11Data.NonDomesticSewageAmount, null);
                salesData.Add(sewageArticle11);
            }

            return salesData;
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
            int[] discount100Percent = [4, 5, 7, 16, 2, 14];//etc
            int[] discount70Percent = [15];
            int[] discount50Percent = [10];

            if (inputDto.DiscountTypeId != null && inputDto.DiscountTypeId > 0)
            {
                if (discount100Percent.Contains(inputDto.DiscountTypeId.Value))
                {
                    IEnumerable<SaleDataOutputDto> result = CalcFinalDiscount(salesData, 100);
                    //discount 100%
                }
                else if (discount70Percent.Contains(inputDto.DiscountTypeId.Value))
                {
                    IEnumerable<SaleDataOutputDto> result = CalcFinalDiscount(salesData, 70);
                    //discount 70%
                }
                else if (discount50Percent.Contains(inputDto.DiscountTypeId.Value))
                {
                    IEnumerable<SaleDataOutputDto> result = CalcFinalDiscount(salesData, 50);
                    //discount 50%
                }
            }

            return salesData;
        }
        private IEnumerable<SaleDataOutputDto> CalcFinalDiscount(IEnumerable<SaleDataOutputDto> salesData, int discountPercent)
        {
            short[] discountOffering = [(short)OfferingEnum.WaterArticle11, (short)OfferingEnum.SewageArticle11, (short)OfferingEnum.WaterInstallation, (short)OfferingEnum.SewageInstalltion];

            foreach (var item in salesData)
            {
                if (discountOffering.Contains(item.Id))
                {
                    item.Discount = (long)(item.Amount * (discountPercent / 100m));
                    item.FinalAmount = item.Amount - (item.Discount ?? 0);
                }
            }

            return salesData;
        }
    }
}
