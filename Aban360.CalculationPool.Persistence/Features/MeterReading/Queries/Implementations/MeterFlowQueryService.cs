using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Implementations
{
    public sealed class MeterFlowQueryService : AbstractBaseConnection, IMeterFlowQueryService
    {
        public MeterFlowQueryService(IConfiguration configuration)
            : base(configuration)
        {
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
        public async Task<MeterFlowValidationDto?> GetMeterFlowValidation(int id)
        {
            string query = GetValidationByIdQuery();
            MeterFlowValidationDto? result = await _sqlReportConnection.QueryFirstOrDefaultAsync<MeterFlowValidationDto>(query, new { id });
            if (result is null || result.Id <= 0)
            {
                throw new ReadingException(ExceptionLiterals.FlowStepNotFound);
            }
            return result;
        }
        public async Task<IEnumerable<MeterFlowCartableGetDto>> GetCartable(IEnumerable<int> zoneIds)
        {
            string query = GetCartablQuery();
            IEnumerable<MeterFlowCartableGetDto> cartable = await _sqlReportConnection.QueryAsync<MeterFlowCartableGetDto>(query, new { zoneIds });

            return cartable;
        }
        public async Task<int> GetFirstFlowId(int latestFlowId)
        {
            string query = GetFirstFlowId();
            int? firstFlowId = await _sqlReportConnection.QueryFirstOrDefaultAsync<int>(query, new { id = latestFlowId });
            if (firstFlowId is null || firstFlowId <= 0)
            {
                throw new ReadingException(ExceptionLiterals.InvalidFlowStep);
            }

            return firstFlowId.Value;
        }
        public async Task<MeterFlowGetDto> GetLatestFlowInfo(int firstFlowId)
        {
            string query = GetLatestFlowId();
            MeterFlowGetDto? latestFlowInfo = await _sqlReportConnection.QueryFirstOrDefaultAsync<MeterFlowGetDto>(query, new { firstFlowId });
            if (latestFlowInfo is null || latestFlowInfo.Id <= 0)
            {
                throw new ReadingException(ExceptionLiterals.InvalidFlowStep);
            }

            return latestFlowInfo;
        }


        private string GetQuery()
        {
            return @"Select 
                        m.Id,
                    	m.MeterFlowStepId,
                    	m.FileName,
                    	m.ZoneId,
                        m.FromReadingNumber,
                        m.ToReadingNumber,
                        m.PrimaryCount,
						t51.C2 as ZoneTitle,
                        m.InsertDateTime,
                        m.RemovedDateTime,
                        m.Description
                    From Atlas.dbo.MeterFlow m
					Left Join Db70.dbo.T51 t51 
						On m.ZoneId=t51.C0
                    Where 
                        m.Id=@id";
        }
        private string GetValidationByFileNameQuery()
        {
            return @"Select InsertDateTime
                    From Atlas.dbo.MeterFlow
                    Where 
                        FileName=@fileName ";
        }
        private string GetValidationByIdQuery()
        {
            return @"Select 
                        Id,
                        MeterFlowStepId,
                        InsertDateTime,
                        RemovedDateTime
                    From Atlas.dbo.MeterFlow
                    Where	
                    	Id=@id";
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
        private string GetLatestFlowId()
        {
            return @"Select top 1 
                        m2.Id,
                    	m2.MeterFlowStepId,
                    	m2.FileName,
                    	m2.ZoneId,
                        m2.FromReadingNumber,
                        m2.ToReadingNumber,
                        m2.PrimaryCount,
                    	t51.C2 as ZoneTitle,
                    	m2.InsertDateTime,
                    	m2.RemovedDateTime,
                    	m2.Description
                    From Atlas.dbo.MeterFlow m1 
                    Join Atlas.dbo.MeterFlow m2
                    	ON m1.FileName=m2.FileName
                    Left Join Db70.dbo.T51 t51 
                    	On m2.ZoneId=t51.C0
                    Where m1.Id=@firstFlowId
                    Order By m2.InsertDateTime Desc";
        }
        private string GetCartablQuery()
        {
            return @"Select  
                    	f.Id,
                    	f.MeterFlowStepId,
                    	fs.Title as StepTitle,
                    	f.FileName,
                    	f.ZoneId,
                        f.FromReadingNumber,
                        f.ToReadingNumber,
                        f.PrimaryCount,
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
                    	f.ZoneId IN @zoneIds AND
                    	f.RemovedByUserId IS NULL AND 
                    	f.RemovedDateTime IS NULL";
        }
    }
}
