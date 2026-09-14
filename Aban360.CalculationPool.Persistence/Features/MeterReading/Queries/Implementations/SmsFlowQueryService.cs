using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Implementations
{
    public sealed class SmsFlowQueryService : AbstractBaseConnection, ISmsFlowQueryService
    {
        public SmsFlowQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<SmsFlowGetDto?> Get(int id, bool hasException)
        {
            string query = GetByIdQuery();
            SmsFlowGetDto? meterSmsFlow = await _sqlReportConnection.QueryFirstOrDefaultAsync<SmsFlowGetDto>(query, new { id });
            if (hasException && meterSmsFlow is null)
            {
                throw new ReadingException(ExceptionLiterals.InvalidSmsFlowId);
            }
            return meterSmsFlow;
        }
        public async Task<SmsFlowGetDto> GetLastByFirstFlowId(int id)
        {
            string query = GetByFirstFlowIdQuery();
            SmsFlowGetDto? meterSmsFlow = await _sqlReportConnection.QueryFirstOrDefaultAsync<SmsFlowGetDto>(query, new { id });
            if (meterSmsFlow is null)
            {
                throw new ReadingException(ExceptionLiterals.InvalidSmsFlowId);
            }
            return meterSmsFlow;
        }
        public async Task<IEnumerable<SmsFlowGetDto>> Get()
        {
            string query = GetQuery();
            IEnumerable<SmsFlowGetDto> result = await _sqlReportConnection.QueryAsync<SmsFlowGetDto>(query);
            return result;
        }

        private string GetQuery()
        {
            return @"SELECT
                        Id,
                        FirstFlowId, 
                        SmsCount, 
                        SmsTemplateId, 
                        InsertDateTime, 
                        InsertBy, 
                        DueDateTime, 
                        SendDateTime 
                    FROM [Atlas].dbo.SmsFlow ";
        }
        private string GetByIdQuery()
        {
            return @"SELECT
                        Id,
                        FirstFlowId, 
                        SmsCount, 
                        SmsTemplateId, 
                        InsertDateTime, 
                        InsertBy, 
                        DueDateTime, 
                        SendDateTime 
                    FROM [Atlas].dbo.SmsFlow
                    WHERE Id = @Id";
        }
        private string GetByFirstFlowIdQuery()
        {
            return @"SELECT Top 1
                        Id,
                        FirstFlowId, 
                        SmsCount, 
                        SmsTemplateId, 
                        InsertDateTime, 
                        InsertBy, 
                        DueDateTime, 
                        SendDateTime 
                    FROM [Atlas].dbo.SmsFlow
                    WHERE FirstFlowId = @id
                    ORDER BY InsertDateTime DESC,Id Desc";
        }
    }
}
