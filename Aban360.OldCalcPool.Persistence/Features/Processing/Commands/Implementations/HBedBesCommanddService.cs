using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Persistence.Constants;
using Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Contracts;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations
{
    public sealed class HBedBesCommanddService 
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public HBedBesCommanddService(
            IDbConnection connection,
            IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Insert(RemoveBillDataInputDto input,string dbName)
        {
            string command = GetInsertCommand(dbName);
            int rowCount = await _connection.ExecuteAsync(command, input, _transaction);
            if (rowCount == 0)
            {
                throw new InvalidBillCommandException(Exceptionliterals.InvalidInsertBillHistory);
            }
        }

        private string GetInsertCommand(string dbName)
        {
            return @$"INSERT INTO [{dbName}].dbo.Hbedbes(
                        TOWN,radif,barge,
                        date_bed,pri_date,today_date,pri_no,today_no,
                        masraf,ab_baha,fas_baha,baha,
                        SH_GHABS1,SH_PARD1,date,operator)
                    VALUES(
                        @ZoneId,@CustomerNumber,@Barge,
                        @RegisterDateJalali,@PreviousDateJalali,@CurrentDateJalali,@PreviousNumber,@CurrentNumber,
                        @Consumption,@AbBahaAmount,@FazelabAmount,@Baha,
                        @BillId,@PaymentId,@ToDayDateJalali,0)";
        }
    }
}
