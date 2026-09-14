using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Queries.Implementations
{
    public sealed class SmsStateTemplateQueryService : AbstractBaseConnection, ISmsStateTemplateQueryService
    {
        public SmsStateTemplateQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<SmsStateTemplateGetDto> Get(short id)
        {
            string query = GetByIdQuery();
            SmsStateTemplateGetDto? meterSmsState = await _sqlReportConnection.QueryFirstOrDefaultAsync<SmsStateTemplateGetDto>(query, new { id });
            if (meterSmsState is null)
            {
                throw new ReadingException(ExceptionLiterals.InvalidSmsStateId);
            }
            return meterSmsState;
        }
        public async Task<SmsStateTemplateGetDto> GetFirst(int groupId,int typeId)
        {
            string query = GetByGroupAndTypeIdQuery();
            SmsStateTemplateGetDto? meterSmsState = await _sqlReportConnection.QueryFirstOrDefaultAsync<SmsStateTemplateGetDto>(query, new { groupId,typeId });
            if (meterSmsState is null)
            {
                throw new ReadingException(ExceptionLiterals.InvalidSmsStateId);
            }
            return meterSmsState;
        }
        public async Task<SmsStateTemplateGetDto?> GetNextStep(int id,bool hasException)
        {
            string query = GetNexByIdQuery();
            SmsStateTemplateGetDto? meterSmsState = await _sqlReportConnection.QueryFirstOrDefaultAsync<SmsStateTemplateGetDto>(query, new { id });
            if (hasException && meterSmsState is null)
            {
                throw new ReadingException(ExceptionLiterals.NotFoundNextStepForSms);
            }
            return meterSmsState;
        }
        public async Task<IEnumerable<SmsStateTemplateGetDto>> Get(bool isValid)
        {
            string query = GetQuery(isValid);
            IEnumerable<SmsStateTemplateGetDto> result = await _sqlReportConnection.QueryAsync<SmsStateTemplateGetDto>(query);
            return result;
        }
        public async Task<SmsStateTemplateGetDto?> GetNextTemplate(int firstFlowId, int groupId)
        {
            string query = GetNextTemplateByGroupAndFirstFlowIdQuery();
            SmsStateTemplateGetDto? meterSmsFlow = await _sqlReportConnection.QueryFirstOrDefaultAsync<SmsStateTemplateGetDto>(query, new { firstFlowId, groupId });
           
            return meterSmsFlow;
        }

        private string GetQuery(bool isValid)
        {
            string removeCondition = isValid ? " WHERE m.RemoveDateTime IS NULL " : string.Empty;
            return @$"SELECT 
                        m.Id,
						m.GroupId,
						g.Title GroupTitle,
                        m.SmsTypeId,
						s.Title SmsTypeTitle,
                        m.StepOrder, 
                        m.SmsText, 
                        m.DueDay,
                        m.Description, 
                        m.InsertDateTime, 
                        m.InsertBy, 
                        m.RemoveDateTime, 
                        m.RemoveBy 
                    FROM [Atlas].dbo.SmsStateTemplate m
					Join [Atlas].dbo.SmsType s
						ON m.SmsTypeId=s.Id
					Join [Atlas].dbo.SmsStateGroup g
						On m.GroupId=g.Id
                    {removeCondition}";
        }
        private string GetByIdQuery()
        {
            return @"SELECT 
                        m.Id,
						m.GroupId,
						g.Title GroupTitle,
                        m.SmsTypeId,
						s.Title SmsTypeTitle,
                        m.StepOrder, 
                        m.SmsText, 
                        m.DueDay,
                        m.Description, 
                        m.InsertDateTime, 
                        m.InsertBy, 
                        m.RemoveDateTime, 
                        m.RemoveBy 
                    FROM [Atlas].dbo.SmsStateTemplate m
					Join [Atlas].dbo.SmsType s
						ON m.SmsTypeId=s.Id
					Join [Atlas].dbo.SmsStateGroup g
						On m.GroupId=g.Id 
                    WHERE m.Id = @Id ";
        }
        private string GetByGroupAndTypeIdQuery()
        {
            return @"SELECT TOP 1
                        m.Id,
						m.GroupId,
						g.Title GroupTitle,
                        m.SmsTypeId,
						s.Title SmsTypeTitle,
                        m.StepOrder, 
                        m.SmsText, 
                        m.DueDay,
                        m.Description, 
                        m.InsertDateTime, 
                        m.InsertBy, 
                        m.RemoveDateTime, 
                        m.RemoveBy 
                    FROM [Atlas].dbo.SmsStateTemplate m
					Join [Atlas].dbo.SmsType s
						ON m.SmsTypeId=s.Id
					Join [Atlas].dbo.SmsStateGroup g
						On m.GroupId=g.Id 
                    WHERE 
                        m.GroupId = @groupId AND
                        m.SmsTypeId = @typeId
                    ORDER By m.StepOrder";
        }
        private string GetNexByIdQuery()
        {
            return @"SELECT 
                        template.Id,
						template.GroupId,
						g.Title GroupTitle,
                        template.SmsTypeId,
						s.Title SmsTypeTitle,
                        template.StepOrder, 
                        template.SmsText, 
                        template.DueDay,
                        template.Description, 
                        template.InsertDateTime, 
                        template.InsertBy, 
                        template.RemoveDateTime, 
                        template.RemoveBy 
                    FROM Atlas.dbo.SmsStateTemplate currentTemplate
                    JOIN Atlas.dbo.SmsStateTemplate template
                        ON template.StepOrder = currentTemplate.StepOrder + 1
					Join [Atlas].dbo.SmsType s
						ON template.SmsTypeId=s.Id
					Join [Atlas].dbo.SmsStateGroup g
						On template.GroupId=g.Id 
                    WHERE 
                    	currentTemplate.Id = @id AND
                        currentTemplate.RemoveDateTime IS NULL AND
                        template.RemoveDateTime IS NULL;";
        }
        private string GetNextTemplateByGroupAndFirstFlowIdQuery()
        {
            return $@"Select Top 1
                    	 t.Id,
                    	 t.GroupId,
                      	 g.Title GroupTitle,
                    	 t.SmsTypeId,
                    	 s.Title SmsTypeTitle,
                    	 t.StepOrder, 
                    	 t.SmsText, 
                    	 t.DueDay,
                    	 t.Description, 
                    	 t.InsertDateTime, 
                    	 t.InsertBy, 
                    	 t.RemoveDateTime, 
                    	 t.RemoveBy 
                    From Atlas.dbo.SmsStateTemplate t
                    Left Join Atlas.dbo.SmsFlow f
                    	ON t.Id=f.SmsTemplateId  AND f.FirstFlowId = @firstFlowId
                    Join [Atlas].dbo.SmsType s
                    	ON t.SmsTypeId=s.Id
                    Join [Atlas].dbo.SmsStateGroup g
                    	On t.GroupId=g.Id
                    Where 
                    	t.GroupId = @groupId AND
                    	f.Id IS NULL AND
                    	t.RemoveDateTime IS NULL
                    Order By t.StepOrder ASC";
        }
    }
}
