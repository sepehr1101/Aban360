using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Implementations
{
    public sealed class MeterSmsStateTemplateQueryService : AbstractBaseConnection, IMeterSmsStateTemplateQueryService
    {
        public MeterSmsStateTemplateQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<MeterSmsStateTemplateGetDto> Get(short id)
        {
            string query = GetByIdQuery();
            MeterSmsStateTemplateGetDto? meterSmsState = await _sqlReportConnection.QueryFirstOrDefaultAsync<MeterSmsStateTemplateGetDto>(query, new { id });
            if (meterSmsState is null)
            {
                throw new ReadingException(ExceptionLiterals.InvalidMeterSmsStateId);
            }
            return meterSmsState;
        }
        public async Task<IEnumerable<MeterSmsStateTemplateGetDto>> Get()
        {
            string query = GetQuery();
            IEnumerable<MeterSmsStateTemplateGetDto> result = await _sqlReportConnection.QueryAsync<MeterSmsStateTemplateGetDto>(query);
            return result;
        }
        private string GetQuery()
        {
            return @"SELECT 
                        Id,
                        SmsTypeId,
                        StepOrder, 
                        SmsText, 
                        DueDay,
                        Description, 
                        InsertDateTime, 
                        InsertBy, 
                        RemoveDateTime, 
                        RemoveBy 
                    FROM [Atlas].dbo.MeterSmsStateTemplate";
        }
        private string GetByIdQuery()
        {
            return @"SELECT 
                        Id,
                        SmsTypeId,
                        StepOrder, 
                        SmsText, 
                        DueDay,
                        Description, 
                        InsertDateTime, 
                        InsertBy, 
                        RemoveDateTime, 
                        RemoveBy 
                    FROM [Atlas].dbo.MeterSmsStateTemplate 
                    WHERE Id = @Id ";
        }
    }
}
