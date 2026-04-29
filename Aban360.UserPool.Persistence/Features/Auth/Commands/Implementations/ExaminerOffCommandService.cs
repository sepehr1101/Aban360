using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Dapper;
using System.Data;

namespace Aban360.UserPool.Persistence.Features.Auth.Commands.Implementations
{
    public class ExaminerOffCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public ExaminerOffCommandService(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Insert(AssessmentOffInsertDto input)
        {
            string command = GetInsertCommand();
            int recordEffected = await _connection.ExecuteAsync(command, input, _transaction);
            if (recordEffected <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertAssessmentOff);
            }
        }
        public async Task Remove(AssessmentOffRemoveDto input)
        {
            string command = GetRemoveCommand();
            int recordEffected = await _connection.ExecuteAsync(command, input, _transaction);
            if (recordEffected <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidRemoveAssessmentOff);
            }
        }
        private string GetInsertCommand()
        {
            return @"Insert AbAndFazelab.dbo.ExaminerOff(
                    	Id,ExaminerCode,ExaminerId,ExaminerName,
                    	JalaliDay,InsertBy,InserterName,
                    	InsertDateTime,InsertDateJalali,InsertTime,
                    	IsCanceled,CanellerName,CancelDateTime,CanellerCode,CancelTime)
                    Values(
                        @Id,@AssessmentCode,@AssessmentId,@AssessmentName,
                    	@OffDateJalali,@InsertedByUserCode,@InsertedByUserName,
                    	@InsertDateGregorian,@InsertDateJalali,@InsertTime,
                    	0,null,null,null,null)";
        }
        private string GetRemoveCommand()
        {
            return @"Update AbAndFazelab.dbo.ExaminerOff
                    Set  
                        IsCanceled=1 ,
                        CancelDateTime=@CancelDateTimeGregorian , 
                        CancelTime=@CancelTime ,
                        CanellerCode=@CancellerCode , 
                        CanellerName=@CancellerName    
                    Where Id=@Id";
        }
    }
}
