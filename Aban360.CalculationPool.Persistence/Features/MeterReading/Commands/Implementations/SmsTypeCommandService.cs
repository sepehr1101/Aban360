using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Dapper;
using System.Data;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Commands.Implementations
{
    public class SmsTypeCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public SmsTypeCommandService(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }
        public async Task Insert(string title)
        {
            string command = GetInsertCommand();
            int effectedRecord = await _connection.ExecuteAsync(command, new { title },_transaction);
            if (effectedRecord <= 0)
            {
                throw new ReadingException(ExceptionLiterals.InvalidInsertSmsType);
            }
        }
        private string GetInsertCommand()
        {
            return @"INSERT INTO Atlas.dbo.SmsType(Title)
                    VALUES(@Title)";
        }
    }
}
