using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class AmountCheckedHandler : IAmountCheckedHandler
    {
        private readonly IMeterFlowValidationGetHandler _meterFlowValidationGetHandler;
        private readonly IMeterFlowService _meterFlowService;
        private const int _expirePercent = 50;
        public AmountCheckedHandler(
            IMeterFlowValidationGetHandler meterFlowValidationGetHandler,
            IMeterFlowService meterFlowService)
        {
            _meterFlowService = meterFlowService;
            _meterFlowService.NotNull(nameof(meterFlowService));

            _meterFlowValidationGetHandler = meterFlowValidationGetHandler;
            _meterFlowValidationGetHandler.NotNull(nameof(meterFlowValidationGetHandler));
        }

        public async Task Handle(int latestFlowId, IAppUser appUser, CancellationToken cancellationToken)
        {
            await _meterFlowValidationGetHandler.Handle(latestFlowId, cancellationToken);
            await MeterFlowComplete(latestFlowId, appUser);
        }
        private async Task UpdateMeterFlow(int latestFlowId, IAppUser appUser)
        {
            MeterFlowUpdateDto meterFlowUpdate = new(latestFlowId, appUser.UserId, DateTime.Now);
            await _meterFlowService.Update(meterFlowUpdate);
        }
        private async Task<int> InsertMeterFlow(MeterFlowStepEnum stepFlowId, int zoneId, string fileName, Guid userId)
        {
            MeterFlowCreateDto newMeterFlow = new()
            {
                MeterFlowStepId = stepFlowId,
                ZoneId = zoneId,
                FileName = fileName,
                InsertByUserId = userId,
                InsertDateTime = DateTime.Now,
            };
            int newMeterFlowId = await _meterFlowService.Create(newMeterFlow);
            return newMeterFlowId;
        }
        private async Task MeterFlowComplete(int latestFlowId, IAppUser appUser)
        {
            await UpdateMeterFlow(latestFlowId, appUser);
            MeterFlowGetDto meterFlow = await _meterFlowService.Get(latestFlowId);

            int calcaltionConfirmedMeterFlow = await InsertMeterFlow(MeterFlowStepEnum.CalculationConfirmed, meterFlow.ZoneId, meterFlow.FileName, appUser.UserId);
        }
    }
}
