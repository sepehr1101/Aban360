using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Constants;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations
{
    public sealed class MandeBedehiCommandService
    {
        private readonly SqlConnection _connection;
        private readonly IDbTransaction _transaction;
        public MandeBedehiCommandService(
                SqlConnection connection,
                IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }


        public async Task UpdateAmount(ZoneIdAndCustomerNumberOutputDto inputDto, long amount, string dbName)
        {
            string command = GetUpdateAmountCommand(dbName);
            int recordCount = await _connection.ExecuteAsync(command, new { inputDto.ZoneId, inputDto.CustomerNumber, amount }, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidCustomerCommandException(Exceptionliterals.InvalidUpdateMandeBedehiAmount);
            }
        }
        private string GetUpdateAmountCommand(string dbName)
        {
            return $@"Update CustomerWarehouse.dbo.MandeBedehi
                    Set Amount=Amount+@Amount
                    Where 
                    	ZoneId=@ZoneId AND
                    	Radif=@CustomerNumber";
        }
    }
}
