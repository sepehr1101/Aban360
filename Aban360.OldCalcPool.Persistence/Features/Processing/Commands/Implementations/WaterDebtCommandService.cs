using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Constants;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations
{
    public sealed class WaterDebtCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public WaterDebtCommandService(
                IDbConnection connection,
                IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }


        public async Task UpdateAmount(string billId, long amount)
        {
            string command = GetUpdateAmountCommand();
            int recordCount = await _connection.ExecuteAsync(command, new { billId, amount }, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidCustomerCommandException(Exceptionliterals.InvalidUpdateWaterDebtAmount);
            }
        }
        private string GetUpdateAmountCommand()
        {
            return $@"Update CustomerWarehouse.dbo.WaterDebt
                    Set Debt=Debt+@Amount
                    Where BillId=@billId";
        }
    }
}
