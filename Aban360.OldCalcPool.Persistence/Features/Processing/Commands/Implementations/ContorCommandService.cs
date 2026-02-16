using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Constants;
using Dapper;
using System.Data;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Commands.Implementations
{
    public sealed class ContorCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public ContorCommandService(
                IDbConnection connection,
                IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task Update(ContorUpdateDto inputDto, string dbName, bool isUpdateTavizField)
        {
            string command = GetUpdateCommand(dbName, isUpdateTavizField);
            int recordCount = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (recordCount <= 0)
            {
                throw new InvalidBillCommandException(Exceptionliterals.InvalidControUpdate);
            }
        }
        private string GetUpdateCommand(string dbName, bool isUpdateTavizField)
        {
            string tavizUpdate = isUpdateTavizField ?
                @"taviz_date=@MeterChangeDateJalali,
                  taviz_no=@MeterChangeNumber," :
                string.Empty;
            return $@"Update [{dbName}].dbo.contor
                    Set
                    	pri_no=@CurrentNumber,
                    	today_no=0,
                    	pri_date=@CurrentDateJalali,
                    	today_date='',
                    	masraf=@Consumption,
                    	{tavizUpdate}
                    	cod_vas=0,
                    	average=@ConsumptionAverage,
                    	mohasbat=2,
                    	operator=5,
                    	mamor=0,
                    	cod_report=0,
                    	old_vas=0,
                    	eslah=0
                    Where town=@ZoneId AND radif=@CustomerNumber  ";
        }
    }
}
