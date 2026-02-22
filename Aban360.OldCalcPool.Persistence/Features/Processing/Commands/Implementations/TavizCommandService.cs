using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Constants;
using Dapper;
using System.Data;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations
{
    public sealed class TavizCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public TavizCommandService(
            IDbConnection connection,
            IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }
        public async Task Insert(TavizInsertDto inputDto, string dbName)
        {
            string command = GetInsertCommand(dbName);
            int recordCount = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidBillCommandException(Exceptionliterals.InvalidInsertMeterChange);
            }
        }

        private string GetInsertCommand(string dbName)
        {
            return $@"Insert [{dbName}].dbo.taviz(
                        town,radif,taviz_no,taviz_date,elat,
                        serial,cod_enshab,enshab,date_sabt,operator
                    )
                Values (
                       @ZoneId,@CustomerNumber,@MeterNumber,@MeterChangeDateJalali,@ChangeCauseId,
                       @BodySerial,@UsageId,@MeterDiameterId,@RegisterDateJalali,@Operator
                    )";
        }
    }
}
