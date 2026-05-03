using Aban360.CalculationPool.Domain.Features.Sale.Dto.Input;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Dapper;
using System.Data;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Commands.Implementations
{
    public class TankerCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public TankerCommandService(
            IDbConnection connection,
            IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Insert(TankerInsertDto input,string dbName)
        {
            string command = GetInsertCommand(dbName);
            int rowEffected = await _connection.ExecuteAsync(command, input, _transaction);
            if (rowEffected <= 0)
            {
                throw new TankerException(ExceptionLiterals.InvalidInsertTanker);
            }
        }
        public async Task Remove(TankerRemoveDto input,string dbName)
        {
            string command = GetRemoveCommand(dbName);
            int rowEffected = await _connection.ExecuteAsync(command, input, _transaction);
            if (rowEffected <= 0)
            {
                throw new TankerException(ExceptionLiterals.InvalidRemoveTanker);
            }
        }

        private string GetInsertCommand(string dbName)
        {
            return @$"Insert [{dbName}].dbo.tanker(
                    	town,radif,name,family,
                    	address,barge,masraf,baha,
                    	date,del,TAG_NOT_SHORB,eshtrak,user_hasf)
                    Values(
                    	@ZoneId ,@CustomerNumber ,@FirstName ,@Surname ,
                    	@Address ,@Barge ,@Consumption ,@Amount ,
                    	@CurrentDateJalali ,0 ,@IsNotShorb ,@ReadingNumber , 0)";
        } 
        private string GetRemoveCommand(string dbName)
        {
            return @$"Update [{dbName}].dbo.tanker
                    Set del=1 , date_hasf=@CurrentDateJalali, user_hasf=@UserCode
                    Where radif=@CustomerNumber";
        }
    }
}
