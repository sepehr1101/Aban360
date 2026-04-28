using Aban360.Common.Db.Dapper;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Persistence.Features.Auth.Queries.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.UserPool.Persistence.Features.Auth.Queries.Implementations
{
    internal sealed class ExaminerOffQueryService : AbstractBaseConnection, IExaminerOffQueryService
    {
        public ExaminerOffQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<AssessmentOffGetDto>> Get(int assessmentCode)
        {
            string query = GetByAssessmentCodeQuery();
            IEnumerable <AssessmentOffGetDto> result = await _sqlReportConnection.QueryAsync<AssessmentOffGetDto>(query, new { assessmentCode , conditionDateJalali = GetStartYear ()});
            return result;
        }
        public async Task<IEnumerable<AssessmentOffGetDto>> Get()
        {
            string query = GetAllQuery();
            IEnumerable<AssessmentOffGetDto> result = await _sqlReportConnection.QueryAsync<AssessmentOffGetDto>(query, new { conditionDateJalali = GetStartYear() });
            return result;
        }
        private string GetStartYear()
        {
            string startYearDateJalali = DateTime.Now.ToShortPersianDateString();
            string[] dateSplit = startYearDateJalali.Split('/');
            return $@"{dateSplit[0]}/01/01";
        }
        private string GetByAssessmentCodeQuery()
        {
            return $@"Select 
                    	Id,
                    	ExaminerCode AssessmentCode,
                    	ExaminerId AssessmentId,
                    	ExaminerName AssessmentName ,
                    	JalaliDay OffDateJalali,
                    	InsertBy InsertedByUserCode ,
                    	InserterName InsertedByUserName,
                    	InsertDateJalali InsertDateJalali ,
                    	InsertDateTime	InsertDateGregorian,
                    	InsertTime InsertedTime ,
                    	IsCanceled IsCanceled,
                    	CancelDateTime CanceledDateGregorian ,
                    	CancelTime CanceledTime,
                    	CanellerCode CancellerCode ,
                    	CanellerName CancellerName
                    From AbAndFazelab.dbo.ExaminerOff
                    Where 
                        ExaminerCode=@assessmentCode AND 
	                    JalaliDay>@conditionDateJalali";
        }
        private string GetAllQuery()
        {
            return $@"Select 
                    	Id,
                    	ExaminerCode AssessmentCode,
                    	ExaminerId AssessmentId,
                    	ExaminerName AssessmentName ,
                    	JalaliDay OffDateJalali,
                    	InsertBy InsertedByUserCode ,
                    	InserterName InsertedByUserName,
                    	InsertDateJalali InsertDateJalali ,
                    	InsertDateTime	InsertDateGregorian,
                    	InsertTime InsertedTime ,
                    	IsCanceled IsCanceled,
                    	CancelDateTime CanceledDateGregorian ,
                    	CancelTime CanceledTime,
                    	CanellerCode CancellerCode ,
                    	CanellerName CancellerName
                    From AbAndFazelab.dbo.ExaminerOff
                    Where JalaliDay>@conditionDateJalali";
        }
    }
}
