using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Implementations
{
    internal sealed class T64QueryService : AbstractBaseConnection, IT64QueryService
    {
        public T64QueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<NumericDictionary>> Get()
        {
            string query = GetAllQuery();
            return await _sqlReportConnection.QueryAsync<NumericDictionary>(query, null);
        }
        public async Task<IEnumerable<AssessmentResultByPreResultOutputDto>> GetAssessment()
        {
            string query = GetAssessmentQuery();
            return await _sqlReportConnection.QueryAsync<AssessmentResultByPreResultOutputDto>(query, null);
        }
        public async Task<IEnumerable<AssessmentResultOutputDto>> GetAll()
        {
            string query = GetAllQuery();
            return await _sqlReportConnection.QueryAsync<AssessmentResultOutputDto>(query, null);
        }
        private string GetAllQuery()
        {
            return @"Select 
                    	C0 Id,
                    	C1 Title,
                        C4 IsSuccess
                    From Db70.dbo.t64
                    WHERE C3=1
                    ORDER BY C0 DESC";                    
        }
        private string GetAssessmentQuery()
        {
            return @"Select 
                    	C0 Id,
                    	C1 Title,
						IsPreResult
                    From Db70.dbo.t64
                    WHERE C3=1 AND IsAssessment=1
                    ORDER BY C0 DESC";                    
        }
    }
}
