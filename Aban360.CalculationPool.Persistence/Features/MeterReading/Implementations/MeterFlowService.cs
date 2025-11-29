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
        public async Task<string?> GetInsertDateTime(string fileName)
        {
            string query = GetValidationByFileNameQuery();
            string? insertDateTime = await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(query, new { fileName });

            return insertDateTime;
        }
        public async Task<string?> GetInsertDateTime(int id)
        {
            string query = GetValidationByIdQuery();
            string? insertDateTime = await _sqlReportConnection.QueryFirstOrDefaultAsync<string>(query, new { id});

            return insertDateTime;
        }
        public async Task<IEnumerable<MeterFlowCartableGetDto>> GetCartable()
        {
            string query = GetCartablQuery();
            IEnumerable<MeterFlowCartableGetDto> cartable=await _sqlReportConnection.QueryAsync<MeterFlowCartableGetDto>(query,null); 
       
            return cartable;
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
                    	m.MeterFlowStepId,
                    	m.FileName,
                    	m.ZoneId,
						t51.C2 as ZoneTitle,
                        m.InsertDateTime
                    From Atlas.dbo.MeterFlow m
					Left Join Db70.dbo.T51 t51 
						On m.ZoneId=t51.C0
                    Where m.Id=@id";
        }
        private string GetValidationByFileNameQuery()
        {
            return @"Select InsertDateTime
                    From Atlas.dbo.MeterFlow
                    Where FileName=@fileName";
        }
        private string GetValidationByIdQuery()
        {
            return @"Select InsertDateTime
                    From Atlas.dbo.MeterFlow
                    Where	
                    	Id=@id AND
                    	RemovedByUserId IS NOT NULL AND
                    	RemovedDateTime IS NOT NULL";
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
        private string GetCartablQuery()
        {
            return @"Select  
                    	f.Id,
                    	f.MeterFlowStepId,
                    	fs.Title as StepTitle,
                    	f.FileName,
                    	f.ZoneId,
						t51.C2 as ZoneTitle,
                    	f.InsertByUserId,
                    	f.InsertDateTime,
                        f.Description
                    From Atlas.dbo.MeterFlow f
                    Join Atlas.dbo.MeterFlowStep fs
                    	On f.MeterFlowStepId=fs.Id
					Left Join Db70.dbo.T51 t51 
						On f.ZoneId=t51.C0
                    Where
                    	--f.ZoneId IN @zoneIds AND
                    	f.RemovedByUserId IS NULL AND 
                    	f.RemovedDateTime IS NULL";
        }
    }
}
