using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.CalculationPool.Persistence.Features.Sale.Commands.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Commands.Implementations;
using Aban360.CalculationPool.Application.Features.Base;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Sale.Dto.Output;
using Aban360.CalculationPool.Persistence.Features.Rule.Queries.Contracts;
using Aban360.CalculationPool.Persistence.Features.Sale.Queries.Contracts;
using Aban360.OldCalcPool.Persistence.Features.Rules.Queries.Contracts;
using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Implementations;

namespace Aban360.CalculationPool.Application.Features.Sale.Services
{
    public interface ITankerWaterOperationsService
    {
        Task TankerWaterSetPayAsync(TankerWaterSetPayInputDto input, CancellationToken cancellationToken);
        Task<TankerWaterCalculationOutputDto> TankerWaterCalculationAsync(TankerWaterCalculationInputDto input, CancellationToken cancellationToken);
    }

    internal sealed class TankerWaterOperationsService : ITankerWaterOperationsService
    {
        private readonly ITankerWaterSetPayCommandService _tankerCommandSerivce;
        private readonly ICustomerInfoService _customerInfoSerivce;
        private readonly ICommonMemberQueryService _commonMemberQuerySerivce;
        private readonly IValidator<TankerWaterSetPayInputDto> _validator;
        private readonly ITankerWaterDistanceTariffQueryService _tankerQueryService;
        private readonly ITankerTariffQueryService _tankerTariffQueryService;
        private readonly IZaribCQueryService _zaribCQueryService;
        private readonly IZaribGetService _zaribGetService;
        public TankerWaterOperationsService(ITankerWaterSetPayCommandService tankerCommandSerivce, ICustomerInfoService customerInfoSerivce, ICommonMemberQueryService commonMemberQuerySerivce, IValidator<TankerWaterSetPayInputDto> validator, ITankerWaterDistanceTariffQueryService tankerQueryService, ITankerTariffQueryService tankerTariffQueryService, IZaribCQueryService zaribCQueryService, IZaribGetService zaribGetService)
        {
            _tankerCommandSerivce = tankerCommandSerivce;
            _tankerCommandSerivce.NotNull(nameof(tankerCommandSerivce));
            _customerInfoSerivce = customerInfoSerivce;
            _customerInfoSerivce.NotNull(nameof(customerInfoSerivce));
            _commonMemberQuerySerivce = commonMemberQuerySerivce;
            _commonMemberQuerySerivce.NotNull(nameof(commonMemberQuerySerivce));
            _validator = validator;
            _validator.NotNull(nameof(validator));
            _tankerQueryService = tankerQueryService;
            _tankerQueryService.NotNull(nameof(tankerQueryService));
            _zaribCQueryService = zaribCQueryService;
            _zaribCQueryService.NotNull(nameof(zaribCQueryService));
            _zaribGetService = zaribGetService;
            _zaribGetService.NotNull(nameof(zaribGetService));
            _tankerTariffQueryService = tankerTariffQueryService;
            _tankerTariffQueryService.NotNull(nameof(tankerTariffQueryService));
        }

        public async Task TankerWaterSetPayAsync(TankerWaterSetPayInputDto input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }

            ZoneIdAndCustomerNumber zoneIdAndCustomerNumber = await _commonMemberQuerySerivce.Get(input.BillId);
            TankerWaterSetPayWithZoneIdAndCustomerNumberInputDto tankerSetPayDto = new(input.PaymentId, zoneIdAndCustomerNumber.ZoneId, zoneIdAndCustomerNumber.CustomerNumber);
            await _tankerCommandSerivce.Insert(tankerSetPayDto);
        }

        public async Task<TankerWaterCalculationOutputDto> TankerWaterCalculationAsync(TankerWaterCalculationInputDto input, CancellationToken cancellationToken)
        {
            TankerCalculationBaseService tankerService = new TankerCalculationBaseService(_tankerQueryService, _tankerTariffQueryService, _zaribCQueryService, _zaribGetService);
            TankerWaterCalculationOutputDto calcResult = await tankerService.Calculate(input, null);
            return calcResult;
        }
    }
}
