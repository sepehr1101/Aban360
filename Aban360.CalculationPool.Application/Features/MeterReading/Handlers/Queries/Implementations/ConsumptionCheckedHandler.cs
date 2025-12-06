using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class ConsumptionCheckedHandler : IConsumptionCheckedHandler
    {
        private readonly IMeterFlowValidationGetHandler _meterFlowValidationGetHandler;
        private readonly IMeterReadingDetailService _meterReadingDetailService;
        private readonly IMeterFlowService _meterFlowService;
        public ConsumptionCheckedHandler(
            IMeterFlowValidationGetHandler meterFlowValidationGetHandler,
            IMeterReadingDetailService meterReadingDetailService,
            IMeterFlowService meterFlowService)
        {
            _meterFlowValidationGetHandler = meterFlowValidationGetHandler;
            _meterFlowValidationGetHandler.NotNull(nameof(meterFlowValidationGetHandler));

            _meterReadingDetailService = meterReadingDetailService;
            _meterReadingDetailService.NotNull(nameof(meterReadingDetailService));

            _meterFlowService = meterFlowService;
            _meterFlowService.NotNull(nameof(meterFlowService));
        }

        public async Task<MeterReadingCheckedOutputDto> Handle(int latestFlowId, IAppUser appUser, CancellationToken cancellationToken)
        {
            await _meterFlowValidationGetHandler.Handle(latestFlowId, MeterFlowStepEnum.Calculated, cancellationToken);
            int newMeterFlowId = await CreateConsumpitonCheckedFlow(latestFlowId, appUser);

            return GetResult(newMeterFlowId);
        }
        private async Task<int> CreateConsumpitonCheckedFlow(int latestFlowId, IAppUser appUser)
        {
            MeterFlowUpdateDto meterFlowUpdate = new(latestFlowId, appUser.UserId, DateTime.Now);
            await _meterFlowService.Update(meterFlowUpdate);

            MeterFlowGetDto meterflow = await _meterFlowService.Get(latestFlowId);
            int newMeterFlowId = await InsertMeterFlow(MeterFlowStepEnum.ConsumptionChecked, meterflow.ZoneId, meterflow.FileName, appUser.UserId, meterflow.Description);

            return newMeterFlowId;
        }
        private async Task<int> InsertMeterFlow(MeterFlowStepEnum stepFlowId, int zoneId, string fileName, Guid userId, string? description)
        {
            MeterFlowCreateDto newMeterFlow = new()
            {
                MeterFlowStepId = stepFlowId,
                ZoneId = zoneId,
                FileName = fileName,
                InsertByUserId = userId,
                InsertDateTime = DateTime.Now,
                Description = description
            };
            int newMeterFlowId = await _meterFlowService.Create(newMeterFlow);
            return newMeterFlowId;
        }
        private MeterReadingCheckedOutputDto GetResult(int flowId)
        {
            return new MeterReadingCheckedOutputDto(flowId, MeterFlowStepEnum.CalculationConfirmed, MessageLiterals.SuccessfullOperation);
        }
    }
}
