using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Implementations;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class ConsumptionCheckedHandler : AbstractBaseConnection, IConsumptionCheckedHandler
    {
        private readonly IMeterFlowValidationGetHandler _meterFlowValidationGetHandler;
        private readonly IMeterReadingDetailQueryService _meterReadingDetailService;
        private readonly IMeterFlowQueryService _meterFlowQueryService;
        public ConsumptionCheckedHandler(
            IMeterFlowValidationGetHandler meterFlowValidationGetHandler,
            IMeterReadingDetailQueryService meterReadingDetailService,
            IMeterFlowQueryService meterFlowQueryService,
            IConfiguration configuration)
            : base(configuration)
        {
            _meterFlowValidationGetHandler = meterFlowValidationGetHandler;
            _meterFlowValidationGetHandler.NotNull(nameof(meterFlowValidationGetHandler));

            _meterReadingDetailService = meterReadingDetailService;
            _meterReadingDetailService.NotNull(nameof(meterReadingDetailService));

            _meterFlowQueryService = meterFlowQueryService;
            _meterFlowQueryService.NotNull(nameof(meterFlowQueryService));
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
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MeterFlowCommandService meterFlowCommandService = new(connection, transaction);
                    await meterFlowCommandService.Update(meterFlowUpdate);
                    MeterFlowGetDto meterflow = await _meterFlowQueryService.Get(latestFlowId);//todo: check and remove from Transaction

                    MeterFlowCreateDto newMeterFlow = new()
                    {
                        MeterFlowStepId = MeterFlowStepEnum.ConsumptionChecked,
                        ZoneId = meterflow.ZoneId,
                        FileName = meterflow.FileName,
                        InsertByUserId = appUser.UserId,
                        InsertDateTime = DateTime.Now,
                        Description = meterflow.Description
                    };
                    int newMeterFlowId = await meterFlowCommandService.Insert(newMeterFlow);

                    transaction.Commit();
                    return newMeterFlowId;
                }
            }
        }
      
        private MeterReadingCheckedOutputDto GetResult(int flowId)
        {
            return new MeterReadingCheckedOutputDto(flowId, MeterFlowStepEnum.CalculationConfirmed, MessageLiterals.SuccessfullOperation);
        }
    }
}
