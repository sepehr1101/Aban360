using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Dapper;
using System.Data;

namespace Aban360.CalculationPool.Persistence.Features.Sale.Commands.Implementations
{
    public class VariablesCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public VariablesCommandService(
            IDbConnection connection,
            IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task<int> GetAndRenewTankerRadif()
        {
            int tankerRadif = await _connection.QueryFirstOrDefaultAsync<int>(GetTankerRadifQuery(), null, _transaction);
            int recordEffected = await _connection.ExecuteAsync(GetIncreaseTankerRadifCommand(), null, _transaction);
            if (recordEffected <= 0)
                throw new TankerException(ExceptionLiterals.InvalidUpdateTankerRadif);

            return tankerRadif;
        }

        private string GetTankerRadifQuery()
        {
            return @"Select TankerRadif From [OldCalc].dbo.Variables";
        }
        private string GetIncreaseTankerRadifCommand()
        {
            return @"Update [OldCalc].dbo.Variables set TankerRadif=TankerRadif+1";
        }
    }
}
