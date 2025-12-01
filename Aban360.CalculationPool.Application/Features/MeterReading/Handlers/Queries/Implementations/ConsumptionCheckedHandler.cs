using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class ConsumptionCheckedHandler : IConsumptionCheckedHandler
    {
        private readonly IMeterFlowValidationGetHandler _meterFlowValidationGetHandler;
        private readonly IMeterReadingDetailService _meterReadingDetailService;
        private readonly IMeterFlowService _meterFlowService;
        private const int _domesticConsumptionExpirePercent = 30;
        private const int _nonDomesticConsumptionExpirePercent = 30;
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

        public async Task Handle(int latestFlowId, IAppUser appUser, CancellationToken cancellationToken)
        {
            await _meterFlowValidationGetHandler.Handle(latestFlowId, cancellationToken);
            await CreateConsumpitonCheckedFlow(latestFlowId, appUser);
        }
        private async Task CreateConsumpitonCheckedFlow(int latestFlowId, IAppUser appUser)
        {
            MeterFlowUpdateDto meterFlowUpdate = new(latestFlowId, appUser.UserId, DateTime.Now);
            await _meterFlowService.Update(meterFlowUpdate);

            MeterFlowGetDto meterflow = await _meterFlowService.Get(latestFlowId);
            await InsertMeterFlow(MeterFlowStepEnum.ConsumptionChecked, meterflow.ZoneId, meterflow.FileName, appUser.UserId, meterflow.Description);
        }
        private async Task InsertMeterFlow(MeterFlowStepEnum stepFlowId, int zoneId, string fileName, Guid userId, string? description)
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
            await _meterFlowService.Create(newMeterFlow);
        }
    }
}
