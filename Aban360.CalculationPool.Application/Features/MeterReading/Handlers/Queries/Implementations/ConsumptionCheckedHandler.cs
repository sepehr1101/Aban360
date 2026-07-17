using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Commands.Implementations;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
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
            MeterFlowUpdateDto meterFlowUpdate = new(latestFlowId, appUser.UserId, DateTime.Now);
            MeterFlowGetDto meterflow = await _meterFlowQueryService.Get(latestFlowId);//todo: Latest check
            MeterFlowCreateDto newMeterFlow = new()
            {
                MeterFlowStepId = MeterFlowStepEnum.ConsumptionChecked,
                FirstFlowId = meterflow.FirstFlowId,
                ZoneId = meterflow.ZoneId,
                FileName = meterflow.FileName,
                FromReadingNumber = meterflow.FromReadingNumber,
                ToReadingNumber = meterflow.ToReadingNumber,
                PrimaryCount = meterflow.PrimaryCount,
                InsertByUserId = appUser.UserId,
                InsertDateTime = DateTime.Now,
                Description = meterflow.Description
            };

            int newMeterFlowId = await ExecSql(meterFlowUpdate, newMeterFlow, latestFlowId, appUser);

            return new MeterReadingCheckedOutputDto(newMeterFlowId, MeterFlowStepEnum.CalculationConfirmed, MessageLiterals.SuccessfullOperation);
        }
        private async Task<int> ExecSql(MeterFlowUpdateDto meterFlowUpdate, MeterFlowCreateDto newMeterFlow, int latestFlowId, IAppUser appUser)
        {
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
                    int newMeterFlowId = await meterFlowCommandService.Insert(newMeterFlow);

                    transaction.Commit();
                    return newMeterFlowId;
                }
            }
        }
    }
}
