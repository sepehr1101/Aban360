using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;
using Aban360.ClaimPool.Persistence.Features.Request.Commands.Contracts;
using Aban360.Common.Db.Dapper;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ClaimPool.Persistence.Features.Request.Commands.Implementations
{
    internal class ExaminationScheduleQueryService : AbstractBaseConnection, IExaminationScheduleQueryService
    {
        public ExaminationScheduleQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<AssessmentScaduleGetDto>> Get(int zoneId, string readingNumber)
        {
            string query = GetByReadingNumberQuery();
            IEnumerable<AssessmentScaduleGetDto> data = await _sqlReportConnection.QueryAsync<AssessmentScaduleGetDto>(query, new { zoneId, readingNumber });
            return data;
        }
        private string GetByReadingNumberQuery()
        {
            return $@"WITH Cte AS(
						Select 
							*,
							Rn=ROW_NUMBER() OVER(Partition By ExaminerCode,FromEshterak Order By InsertDateTime Desc)
						From AbAndFazelab.dbo.ExaminationSchedule 
						where
							ZoneId=@ZoneId AND
							(@ReadingNumber BETWEEN FromEshterak AND ToEshterak) AND
							IsActive=1
					)
					Select 
						ExaminerId AssessmentId,
						ExaminerCode AssessmentCode,
						ExaminerName AssessmentName,
						ZoneId,
						ZoneTitle,
						IsRoosta IsVillage,
						FromEshterak FromReadingNumber,
						ToEshterak ToReadingNumber,
						IsActive, 
						Day0,
						Day1,
						Day2,
						Day3,
						Day4,
						Day5,
						Day6,
						Day7
					From Cte
					Where Rn=1";
        }
    }
}
