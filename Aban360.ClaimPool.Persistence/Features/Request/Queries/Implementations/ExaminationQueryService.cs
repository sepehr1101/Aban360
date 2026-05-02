using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Implementations
{
    internal sealed class ExaminationQueryService : AbstractBaseConnection, IExaminationQueryService
    {
        public ExaminationQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<AssessmentGetDto> Get(int code)
        {
            string query = GetSingleQuery();
            AssessmentGetDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<AssessmentGetDto>(query, new { code });
            return result;
        }
        public async Task<AssessmentDataOutputDto> Get(Guid id)
        {
            string query = GetByIdQuery();
            AssessmentDataOutputDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<AssessmentDataOutputDto>(query, new { id });
            return result;
        }
        public async Task<bool> HasResultByTrackId(Guid trackId)
        {
            string query = GetHasResultByTrackIdQuery();
            bool hasResult = await _sqlReportConnection.QueryFirstOrDefaultAsync<bool>(query, new { trackId });
            return hasResult;
        }
        public async Task<int> GetWithoutResultInDate(string assessmentDateJalai, int assessmentCode)
        {
            string query= GetWithoutResultInDateQuery();
            int assessmentTaskCount=await _sqlReportConnection.QueryFirstOrDefaultAsync<int>(query, new { assessmentDateJalai, assessmentCode });
            return assessmentTaskCount;
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
        private string GetByIdQuery()
        {
            return @"Select 
                    	Id,
                    	TrackNumber,
                    	BillId,
                    	ExaminerCode AssessmentCode ,
                    	ExaminerName AssessmentName,
                    	ExaminerMobile AssessmentMobile,
                    	DayJalali AssessmentDateJalali,
                    	DayMiladi AssessmentGregorianDateTime,
                    	ZoneId,
                    	t51.C2 ZoneTitle,
                    	ResultId,
                    	t64.C1 ResultTitle,
                    	SetResultDateTime SetResultDateTime,
                    	ResultDescription Description,
                    	TrackId ,
                    	TrackIdResult,
                    	X1,
                    	Y1,
                    	X2,
                    	Y2,
                    	Eshterak ReadingNumber,
                    	Arse Premises,
                    	ArzeshMelk HouserValue,
                    	KarbariId UsageId,
                    	t41.C1 UsageTitle,
                    	AllInJson
                    From AbAndFazelab.dbo.Examination 
                    Left Join [Db70].dbo.T51 t51
                    	ON ZoneId=t51.C0
                    Left Join [Db70].dbo.T41 t41
                    	ON KarbariId=t41.C0
                    Left Join [Db70].dbo.T64 t64
                    	ON ResultId=t64.C0
                    Where Id=@id";
        }
        private string GetHasResultByTrackIdQuery()
        {
            return $@"Select IIF(TrackIdResult is Null,0,1)
                    From AbAndFazelab.dbo.Examination 
                    Where TrackId=@trackId";
        }
        private string GetWithoutResultInDateQuery()
        {
            return $@"Select COUNT(1)
                    From AbAndFazelab.dbo.Examination
                    Where 
                    	ExaminerCode=@assessmentCode AND
                    	DayJalali =@assessmentDateJalai AND
                    	TrackIdResult IS NULL";
        }
    }
}
