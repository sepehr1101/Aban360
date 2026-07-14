using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Implementations
{
    public sealed class MeterSmsFlowQueryService : AbstractBaseConnection, IMeterSmsFlowQueryService
    {
        public MeterSmsFlowQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<MeterSmsFlowGetDto> Get(int id)
        {
            string query = GetByIdQuery();
            MeterSmsFlowGetDto? meterSmsFlow = await _sqlReportConnection.QueryFirstOrDefaultAsync<MeterSmsFlowGetDto>(query, new { id });
            if (meterSmsFlow is null)
            {
                throw new ReadingException(ExceptionLiterals.InvalidMeterSmsFlowId);
            }
            return meterSmsFlow;
        }
        public async Task<IEnumerable<MeterSmsFlowGetDto>> Get()
        {
            string query = GetQuery();
            IEnumerable<MeterSmsFlowGetDto> result = await _sqlReportConnection.QueryAsync<MeterSmsFlowGetDto>(query);
            return result;
        }
        private string GetQuery()
        {
            return @"SELECT
                        Id,
                        FlowId, 
                        SmsCount, 
                        SmsTemplateId, 
                        InsertDateTime, 
                        InsertBy, 
                        DueDateTime, 
                        SendDateTime 
                    FROM Atlas.dbo.MeterSmsFlow 
                    WHERE Id = @Id";
        }
        private string GetByIdQuery()
        {
            return @"SELECT
                        Id,
                        FlowId, 
                        SmsCount, 
                        SmsTemplateId, 
                        InsertDateTime, 
                        InsertBy, 
                        DueDateTime, 
                        SendDateTime 
                    FROM Atlas.dbo.MeterSmsFlow ";
        }
    }
}
