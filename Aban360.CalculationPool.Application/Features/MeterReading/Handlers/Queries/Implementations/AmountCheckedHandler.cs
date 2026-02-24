using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Implementations;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class AmountCheckedHandler : AbstractBaseConnection, IAmountCheckedHandler
    {
        private readonly IMeterFlowValidationGetHandler _meterFlowValidationGetHandler;
        private readonly IMeterFlowQueryService _meterFlowQueryService;
        private const int _expirePercent = 50;
        public AmountCheckedHandler(
            IMeterFlowValidationGetHandler meterFlowValidationGetHandler,
            IMeterFlowQueryService meterFlowQueryService,
            IConfiguration configuration)
         : base(configuration)
        {
            _meterFlowQueryService = meterFlowQueryService;
            _meterFlowQueryService.NotNull(nameof(meterFlowQueryService));

            _meterFlowValidationGetHandler = meterFlowValidationGetHandler;
            _meterFlowValidationGetHandler.NotNull(nameof(meterFlowValidationGetHandler));
        }

        public async Task Handle(int latestFlowId, IAppUser appUser, CancellationToken cancellationToken)
        {
            await _meterFlowValidationGetHandler.Handle(latestFlowId, cancellationToken);
            await MeterFlowComplete(latestFlowId, appUser);
        }
        private async Task<int> MeterFlowComplete(int latestFlowId, IAppUser appUser)
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
                    MeterFlowGetDto meterFlow = await _meterFlowQueryService.Get(latestFlowId);//todo: check and remove from Transaction

                    MeterFlowCreateDto newMeterFlow = new()
                    {
                        MeterFlowStepId = MeterFlowStepEnum.CalculationConfirmed,
                        ZoneId = meterFlow.ZoneId,
                        FileName = meterFlow.FileName,
                        InsertByUserId = appUser.UserId,
                        InsertDateTime = DateTime.Now,
                        Description = meterFlow.Description
                    };
                    int newMeterFlowId = await meterFlowCommandService.Insert(newMeterFlow);

                    transaction.Commit();
                    return newMeterFlowId;
                }
            }
        }
    }
}
