using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
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
        public async Task Update(MeterFlowUpdateDto input)
        {
            string query = GetUpdateCommand();
            await _sqlReportConnection.ExecuteAsync(query, input);
        }
        public async Task<MeterFlowGetDto> Get(int id)
        {
            string query = GetQuery();
            MeterFlowGetDto meterFlow = await _sqlReportConnection.QueryFirstOrDefaultAsync<MeterFlowGetDto>(query, new { id });

            return meterFlow;
        }
        public async Task<string?> Get(string fileName)
        {
            string query = GetValidationQuery();
            string? insertDateTime = await _sqlReportConnection.QueryFirstAsync<string>(query, new { fileName });

            return insertDateTime;
        }
        public async Task<int> GetFirstFlowId(int latestFlowId)
        {
            string query = GetFirstFlowId();
            int firstFlowId = await _sqlReportConnection.QueryFirstOrDefaultAsync<int>(query, new { id = latestFlowId });

            return firstFlowId;
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
        private string GetUpdateCommand()
        {
            return @"Update Atlas.dbo.MeterFlow
                        Set RemovedDateTime=@RemovedDateTime , RemovedByUserId=@RemovedByUserId
                        Where Id=@id";
        }
        private string GetQuery()
        {
            return @"Select 
                    	MeterFlowStepId,
                    	FileName,
                    	ZoneId
                    From Atlas.dbo.MeterFlow
                    Where Id=@id";
        }
        private string GetValidationQuery()
        {
            return @"Select InsertDateTime
                    From Atlas.dbo.MeterFlow
                    Where FileName=@fileName";
        }
        private string GetFirstFlowId()
        {
            return @"select f1.Id
                    From Atlas.dbo.MeterFlow f2
                    Join Atlas.dbo.MeterFlow f1
                    	On f2.FileName=f1.FileName
                    Where
                    	f2.Id=@id AND
                    	f1.MeterFlowStepId=1";
        }
    }
}
