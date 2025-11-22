using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Implementations
{
    internal sealed class MeterFlowService : AbstractBaseConnection, IMeterFlowService
    {
        public MeterFlowService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<int> Create(MeterFlowCreateDto input)
        {
            string command = GetInsertCommand();
            int id = await _sqlReportConnection.ExecuteScalarAsync<int>(command, input);

            return id;
        }
        private string GetInsertCommand()
        {
            return @"INSERT [Atlas].[dbo].[MeterFlow] 
                        (
                            MeterFlowStepId,FileName,ZoneId,
                            InsertDateTime,InsertByUserId,Description
                        )
                    VALUES 
                        (
                            @MeterFlowStepId,@FileName,@ZoneId,
                            @InsertDateTime,@InsertByUserId,@Description
                        );
                    SELECT CAST(SCOPE_IDENTITY() AS int);";
        }
    }
}
