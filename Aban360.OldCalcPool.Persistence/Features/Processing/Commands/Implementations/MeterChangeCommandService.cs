using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Constants;
using Dapper;
using System.Data;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations
{
    public sealed class MeterChangeCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public MeterChangeCommandService(
            IDbConnection connection,
            IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }
        public async Task Insert(MeterChangeInsertDto inputDto)
        {
            string command = GetInsertCommand();
            int recordCount = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidBillCommandException(Exceptionliterals.InvalidInsertMeterChange);
            }
        }
        private string GetInsertCommand()
        {
            return @"Insert CustomerWarehouse.dbo.MeterChange(
                        ZoneId,CustomerNumber,MeterNumber,ChangeDateJalali,
                        ChangeCauseId,ChangeCauseTitle,RegisterDateJalali,BodySerial
                    )
                    Values(
                        @ZoneId,@CustomerNumber,@MeterNumber,@MeterChangeDateJalali,
                        @ChangeCauseId,@ChangeCauseTitle,@RegisterDateJalali,@BodySerial
                    )";
        }
    }
}
