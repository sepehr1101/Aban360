using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Constants;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations
{
    public sealed class BillCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public BillCommandService(
                IDbConnection connection,
                IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task InsertByBedBesId(ZoneIdAndCustomerNumberOutputDto inputDto, int bedBesId, string dbName)
        {
            string command = GetInsertByBedBesCommand(dbName);
            int recordCount = await _connection.ExecuteAsync(command, new { inputDto.CustomerNumber, inputDto.ZoneId, bedBesId }, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidBillCommandException(Exceptionliterals.InvalidBillInsert);
            }
        }
        private string GetInsertByBedBesCommand(string dbName)//todo
        {
            return $@"";
        }
    }
}
