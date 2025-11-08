using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
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

            IEnumerable<SaleDataOutputDto> salesData = await GetSalesData(inputDto);
            EquipmentBrokerOutputDto equipmentBroker = await _equipmentBrokerAndZoneQueryService.Get(inputDto.ZoneId);
            ReportOutput<SaleHeaderOutputDto, SaleDataOutputDto> finalSale = CalcSale(salesData, equipmentBroker != null && equipmentBroker.Id > 0 ? true : false);

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
        }
        private ReportOutput<SaleHeaderOutputDto, SaleDataOutputDto> CalcSale(IEnumerable<SaleDataOutputDto> salesData, bool hasBroker)
        {
            SaleHeaderOutputDto headerOutput = new SaleHeaderOutputDto()
            {//todo: review
                HasBroker = hasBroker,
                BrokerAmount = hasBroker ? salesData.Where(s => s.Id == (short)OfferingEnum.WaterEquipment || s.Id == (short)OfferingEnum.SewageEquipment).Sum(s => s.Amount) : 0,
                BrokerOfferingCount = hasBroker ? salesData.Where(s => s.Id == (short)OfferingEnum.WaterEquipment || s.Id == (short)OfferingEnum.SewageEquipment).Count() : 0,
                CompanyAmount = !hasBroker ? salesData.Where(s => s.Id != (short)OfferingEnum.WaterEquipment || s.Id != (short)OfferingEnum.SewageEquipment).Sum(s => s.Amount) : salesData.Sum(s => s.Amount),
                CompanyOfferingCount = !hasBroker ? salesData.Where(s => s.Id != (short)OfferingEnum.WaterEquipment || s.Id != (short)OfferingEnum.SewageEquipment).Count() : salesData.Count(),
                OfferingCount = salesData.Count(),
                OfferingAmount = salesData.Sum(s => s.Amount),
            };

            ReportOutput<SaleHeaderOutputDto, SaleDataOutputDto> result = new(title, headerOutput, salesData);
            return result;
        }
        private async Task<IEnumerable<SaleDataOutputDto>> GetSalesData(SaleInputDto inputDto)
        {
            List<SaleDataOutputDto> salesData = new List<SaleDataOutputDto>();

            ICollection<SaleDataOutputDto> waterInstallationAndEquipment = await GetInstallationAndEquipment(true, inputDto.WaterDiameterId);
            salesData.AddRange(waterInstallationAndEquipment);

            var article11 = new Article11GetDto(inputDto.ZoneId, inputDto.Block, DateTime.Now.ToShortPersianDateString());
            Article11OutputDto article11Data = await _article11QueryService.Get(article11);
            SaleDataOutputDto waterArticle11 = await GetSaleData(OfferingEnum.WaterArticle11,inputDto.IsDomestic? article11Data.DomesticWaterAmount: article11Data.NonDomesticWaterAmount, null);
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
            SaleDataOutputDto saleData = new((short)offeringEnum, title, amount, discount);

            return saleData;
        }
    }
}
