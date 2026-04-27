using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Implementations
{
    internal sealed class AssessmentQueryService : AbstractBaseConnection, IAssessmentQueryService
    {
        public AssessmentQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<AssessmentGetDto> Get(int code)
        {
            string query = GetSingleQuery();
            AssessmentGetDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<AssessmentGetDto>(query, new { code });
            return result;
        }
        public async Task<bool> HasResultByTrackId(Guid trackId)
        {
            string query = GetHasResultByTrackIdQuery();
            bool hasResult = await _sqlReportConnection.QueryFirstOrDefaultAsync<bool>(query, new { trackId });
            return hasResult;
        }
        private string GetSingleQuery()
        {
            return @"Select Top 1
                    	ExaminerCode Code,
                    	ExaminerName FullName,
                    	ExaminerMobile PhoneNumber
                    From AbAndFazelab.dbo.Examination
                    Where ExaminerCode=@code";
        }
        private string GetHasResultByTrackIdQuery()
        {
            return $@"Select IIF(TrackIdResult is Null,0,1)
                    From AbAndFazelab.dbo.Examination 
                    Where TrackId=@trackId";
        }
    }
}
