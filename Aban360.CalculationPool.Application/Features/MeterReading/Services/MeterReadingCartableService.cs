using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.CollectBills.Inputs;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Services;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using FluentValidation;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Services
{
    public interface IMeterReadingCartableService
    {
        Task<IEnumerable<MeterFlowCartableGetDto>> CartableByZoneIdGetAsync(MeterFlowByZoneInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
        Task<IEnumerable<MeterFlowCartableGetDto>> CartableAsync(IAppUser appUser, CancellationToken cancellationToken);
    }

    internal sealed class MeterReadingCartableService : IMeterReadingCartableService
    {
        private readonly IMeterFlowQueryService _meterFlowQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IValidator<MeterFlowByZoneInputDto> _validator;
        public MeterReadingCartableService(IMeterFlowQueryService meterFlowQueryService, ICommonZoneService commonZoneService, IValidator<MeterFlowByZoneInputDto> validator)
        {
            _meterFlowQueryService = meterFlowQueryService;
            _meterFlowQueryService.NotNull(nameof(meterFlowQueryService));
            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));
            _validator = validator;
            _validator.NotNull(nameof(validator));
        }

        public async Task<IEnumerable<MeterFlowCartableGetDto>> CartableByZoneIdGetAsync(MeterFlowByZoneInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken)
        {
            await CartableByZoneIdGetInputValidate(inputDto, cancellationToken);
            await _commonZoneService.IsUserInZone(appUser, inputDto.ZoneId);
            IEnumerable<MeterFlowCartableGetDto> result = await _meterFlowQueryService.GetCartable(inputDto, MeterFlowStepEnum.CalculationConfirmed);
            return result;
        }

        private async Task CartableByZoneIdGetInputValidate(MeterFlowByZoneInputDto inputDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(inputDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                var message = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                throw new CustomValidationException(message);
            }
        }

        public async Task<IEnumerable<MeterFlowCartableGetDto>> CartableAsync(IAppUser appUser, CancellationToken cancellationToken)
        {
            IEnumerable<int> zoneIds = await _commonZoneService.GetMyZoneIds(appUser);
            IEnumerable<MeterFlowCartableGetDto> cartable = await _meterFlowQueryService.GetCartable(zoneIds);
            return cartable;
        }
    }
}
