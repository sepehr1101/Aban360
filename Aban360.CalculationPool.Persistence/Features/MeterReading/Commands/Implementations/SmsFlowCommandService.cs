using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Dapper;
using System.Data;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Commands.Implementations
{
    public sealed class SmsFlowCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public SmsFlowCommandService(
            IDbConnection connection,
            IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task<int> Insert(SmsFlowInsertDto input)
        {
            string command = GetInsertCommand();
            int id = await _connection.ExecuteScalarAsync<int>(command, input, _transaction);
            if (id <= 0)
            {
                throw new ReadingException(ExceptionLiterals.InvalidSmsFlowId);
            }

            return id;
        }
        public async Task Insert(ICollection<SmsFlowInsertDto> input)
        {
            string command = GetInsertCommand();
            await _connection.ExecuteAsync(command, input, _transaction);

        }
        public async Task Update(SmsFlowUpdateDto input)
        {
            string command = GetUpdateCommand();
            int rowEffected = await _connection.ExecuteAsync(command, input, _transaction);
            if (rowEffected <= 0)
            {
                throw new ReadingException(ExceptionLiterals.InvalidUpdate);
            };
        }
        private string GetInsertCommand()
        {
            return @"INSERT INTO [Atlas].dbo.SmsFlow
                    (
                         FirstFlowId, SmsCount, SmsTemplateId, 
                         InsertDateTime, InsertBy, DueDateTime
                    ) 
                    VALUES 
                    (
                        @FirstFlowId, @SmsCount, @SmsTemplateId,
                        @InsertDateTime, @InsertBy, @DueDateTime
                    );


                SELECT CAST(SCOPE_IDENTITY() AS SMALLINT);";
        }
        private string GetUpdateCommand()
        {
            return @"Update [Atlas].dbo.SmsFlow
                    Set SendDateTime = @SendDateTime
                    Where Id = @Id";
        }
    }
}
