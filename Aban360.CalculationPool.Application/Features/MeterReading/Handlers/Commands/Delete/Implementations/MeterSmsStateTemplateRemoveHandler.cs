using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Commands.Implementations;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Delete.Implementations
{
    internal sealed class MeterSmsStateTemplateRemoveHandler : AbstractBaseConnection, IMeterSmsStateTemplateRemoveHandler
    {
        public MeterSmsStateTemplateRemoveHandler(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task Handle(short id, IAppUser appUser, CancellationToken cancellationToken)
        {
            MeterSmsStateTemplateRemoveDto removeDto = new(id, appUser.UserId);
            await ExecSql(removeDto);
        }
        private async Task ExecSql(MeterSmsStateTemplateRemoveDto inputDto)
        {
            using (IDbConnection connection = _sqlReportConnection)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    MeterSmsStateTemplateCommandService meterSmsStateCommandService = new(connection, transaction);
                    await meterSmsStateCommandService.Delete(inputDto);

                    transaction.Commit();
                }
            }
        }
    }
}
