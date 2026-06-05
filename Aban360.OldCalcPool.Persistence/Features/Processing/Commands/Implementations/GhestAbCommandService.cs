using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Constants;
using Dapper;
using System.Data;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations
{
    public sealed class GhestAbCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public GhestAbCommandService(
            IDbConnection connection,
            IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Insert(IEnumerable<BillInstallmentCreateDto> inputDto, string dbName)
        {
            string command = GetInsertCommand(dbName);
            int recordCount = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (inputDto.Count() > 0 && recordCount <= 0)
            {
                throw new InvalidBillCommandException(Exceptionliterals.InvalidGhestAbInsert);
            }
        }
        public async Task Update(BillInstallmentUpdateDto inputDto, string dbName)
        {
            string command = GetUpdateCommand(dbName);
            int recordCount = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidBillCommandException(Exceptionliterals.InvalidGhestAbUpdate);
            }
        }

        private string GetInsertCommand(string dbName)
        {
            return $@"Insert [{dbName}].dbo.ghest_ab(
                    	town,radif,eshtrak,barge,
                    	date_bed,mohlat,pard,cod_enshab,
                    	enshab,serial,sabt,operator)
                    Values(
                    	@ZoneId,@CustomerNumber,@ReadingNumber,@barge,
                    	@RegisterDateJalali,@DeadLineDateJalali,@Payable,@UsageId,
                    	@MeterDiameterId,@QueueNumber,0,@Operator)";
        }
        private string GetUpdateCommand(string dbName)
        {
            return $@"Update [{dbName}].dbo.ghest_ab 
                    Set mohlat=@DeadLineDateJalali , pard=@amount
                    where 
                    	ID=@id AND
                    	town=@zoneId AND
                    	radif=@customerNumber";
        }

    }
}
