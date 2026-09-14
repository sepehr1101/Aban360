using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Dapper;
using System.Data;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Commands.Implementations
{
    public sealed class SmsStateTemplateCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public SmsStateTemplateCommandService(
            IDbConnection connection,
            IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task<short> Insert(SmsStateTemplateInsertDto input)
        {
            string command = GetInsertCommand();
            short id = await _connection.ExecuteScalarAsync<short>(command, input, _transaction);
            if (id <= 0)
            {
                throw new ReadingException(ExceptionLiterals.InvalidSmsStateId);
            }

            return id;
        }
        public async Task Delete(SmsStateTemplateRemoveDto input)
        {
            string command = GetRemoveCommand();
            int rowEffected = await _connection.ExecuteAsync(command, input, _transaction);
            if (rowEffected <= 0)
            {
                throw new ReadingException(ExceptionLiterals.InvalidRemove);
            };
        }
        private string GetInsertCommand()
        {
            return @"INSERT INTO [Atlas].dbo.SmsStateTemplate 
                    (
                         GroupId, SmsTypeId, StepOrder, 
                         SmsText, DueDay, Description,
                         InsertDateTime, InsertBy
                    ) 
                    VALUES 
                    (
                        @GroupId, @SmsTypeId, @StepOrder,
                        @SmsText, @DueDay, @Description, 
                        @InsertDateTime, @InsertBy
                    )

                    SELECT CAST(SCOPE_IDENTITY() AS smallint);";
        }
        private string GetRemoveCommand()
        {
            return @"Update [Atlas].dbo.SmsStateTemplate	
                    Set 
                    	RemoveBy=@RemoveBy ,
                    	RemovedDateTime=@RemovedDateTime
                    Where Id=@Id";
        }
    }
}
