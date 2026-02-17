using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Aban360.OldCalcPool.Persistence.Constants;
using Dapper;
using System.Data;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations
{
    public sealed class RemovedBillCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public RemovedBillCommandService(
                IDbConnection connection,
                IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Insert(ZoneIdAndCustomerNumberOutputDto inputDto, long barge, string dbName)
        {
            string command = GetInsertCommand(dbName);
            int recordCount = await _connection.ExecuteAsync(command, new { inputDto.ZoneId, inputDto.CustomerNumber, barge }, _transaction);
            if (recordCount == 0)
            {
                throw new InvalidBillCommandException(Exceptionliterals.InvalidInsertBillHistory);
            }
        }
        private string GetInsertCommand(string dbName)
        {
            return $@"insert into [CustomerWarehouse].dbo.RemovedBills  
                    (
                        ZoneId,
                        ZoneTitle,
                        CustomerNumber,
                        BillId,
                        PreviousNumber,
                        NextNumber,
                        PreviousDay,
                        NextDay,
                        RegisterDay,
                        RegisterDayGregorian,
                        Consumption,
                        SumItems,
                        RemoveDay,
                        RemoveDayGregorian,
                        ZoneId2
                    )
                    Select
                        b.town ZoneId,
                        z.C2 ZoneTitle,
                        b.radif CustomerNumber,
                        b.sh_ghabs1 BillId,
                        b.pri_no PreviousNumber,
                        b.today_no NextNumber,
                        b.pri_date PreviousDay,
                        b.today_date NextDay,
                        b.date_bed RegisterDay,
                        AbAndFazelab.dbo.PersianToMiladi(b.date_bed) RegisterDayGregorian,
                        b.masraf Consumption,
                        b.baha SumItems,
                        b.date RemoveDay,
                        AbAndFazelab.dbo.PersianToMiladi(b.date) RemoveDayGregorian,
                        z.C0
                    From [{dbName}].dbo.Hbedbes b
                    Join Db70.dbo.T51 z
                    On b.town=z.C0
                    WHERE 
                    	SH_GHABS1<>'' AND
                    	b.TOWN=@zoneId AND
                    	b.radif=@customerNumber AND 
                    	b.barge=@barge";
        }
    }
}
