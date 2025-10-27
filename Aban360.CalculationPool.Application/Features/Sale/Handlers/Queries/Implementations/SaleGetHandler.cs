using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;

namespace Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Implementations
{
    internal sealed class SaleGetHandler : ISaleGetHandler
    {
        private readonly IInstallationAndEquipmentQueryService _installationAndEquipmentService;
        private readonly IArticle11QueryService _article11QueryService;
        private readonly IEquipmentBrokerAndZoneQueryService _equipmentBrokerAndZoneQueryService;
        private readonly IValidator<SaleInputDto> _validator;
        public SaleGetHandler(
            IInstallationAndEquipmentQueryService installationAndEquipmentService,
            IArticle11QueryService article11QueryService,
            IEquipmentBrokerAndZoneQueryService equipmentBrokerAndZoneQueryService,
            IValidator<SaleInputDto> validator)
        {
            _installationAndEquipmentService = installationAndEquipmentService;
            _installationAndEquipmentService.NotNull(nameof(installationAndEquipmentService));

            _article11QueryService = article11QueryService;
            _article11QueryService.NotNull(nameof(article11QueryService));

            _equipmentBrokerAndZoneQueryService = equipmentBrokerAndZoneQueryService;
            _equipmentBrokerAndZoneQueryService.NotNull(nameof(equipmentBrokerAndZoneQueryService));

            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<SaleOutputDto> Handle(SaleInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            SaleOutputDto saleOutput = await GetSale(inputDto);

            EquipmentBrokerOutputDto equipmentBroker = await _equipmentBrokerAndZoneQueryService.Get(inputDto.ZoneId);
            SaleOutputDto finalSale = CalcSale(saleOutput, equipmentBroker.Id > 0 ? true : false);

            return finalSale;
        }
        private SaleOutputDto CalcSale(SaleOutputDto inputDto, bool hasBroker)
        {
            inputDto.HasBroker = hasBroker;

            inputDto.CompanyAmount = inputDto.WaterInstallationAmount +
                inputDto.Article11WaterMeterAmount +
                inputDto.Article11WaterAmount +
                (inputDto.SewageInstallationAmount ?? 0) +
                (inputDto.Article11SewageMeterAmount ?? 0) +
                (inputDto.Article11SewageAmount ?? 0);

            long equipment = inputDto.WaterEquipmentAmount + (inputDto.SewageEquipmentAmount ?? 0);

            if (hasBroker)
            {
                inputDto.BrokerAmount = equipment;
            }
            else
            {
                inputDto.CompanyAmount += equipment;
            }

            return inputDto;
        }
        private async Task<SaleOutputDto> GetSale(SaleInputDto inputDto)
        {
            var waterInstallationAndEquipment = new InstallationAndEquipmentGetDto(true, inputDto.WaterDiameterId);
            InstallationAndEquipmentOutputDto waterInstalltionAndEquipmentData = await _installationAndEquipmentService.Get(waterInstallationAndEquipment);

            var article11 = new Article11GetDto(inputDto.ZoneId, inputDto.IsDomestic, inputDto.Block);
            Article11OutputDto article11Data = await _article11QueryService.Get(article11);


            var saleOutput = new SaleOutputDto()
            {
                WaterInstallationAmount = waterInstalltionAndEquipmentData.InstallationAmount,
                WaterEquipmentAmount = waterInstalltionAndEquipmentData.EquipmentAmount,
                Article11WaterMeterAmount = article11Data.WaterMeterAmount,
                Article11WaterAmount = article11Data.WaterAmount,
            };

            if (HasSiphon(inputDto))
            {
                var sewageInstallationAndEquipment = new InstallationAndEquipmentGetDto(false, inputDto.SiphonDiameterId);
                InstallationAndEquipmentOutputDto sewageInstalltionAndEquipmentData = await _installationAndEquipmentService.Get(sewageInstallationAndEquipment);

                saleOutput.SewageInstallationAmount = sewageInstalltionAndEquipmentData.InstallationAmount;
                saleOutput.SewageEquipmentAmount = sewageInstalltionAndEquipmentData.EquipmentAmount;
                saleOutput.Article11SewageMeterAmount = article11Data.SewageMeterAmount;
                saleOutput.Article11SewageAmount = article11Data.SewageAmount;
            }

            return saleOutput;
        }
        private bool HasSiphon(SaleInputDto input)
        {
            return input.SiphonDiameterId != null && input.SiphonDiameterId > 0 ? true : false;
        }
    }
}
