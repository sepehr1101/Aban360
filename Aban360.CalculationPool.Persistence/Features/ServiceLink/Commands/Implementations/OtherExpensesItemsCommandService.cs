using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Dapper;
using System.Data;

namespace Aban360.CalculationPool.Persistence.Features.ServiceLink.Commands.Implementations
{
    public sealed class OtherExpensesItemsCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public OtherExpensesItemsCommandService(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }
        public async Task Insert(OtherExpensesItemsInsertDto inputDto)
        {
            string command = GetInsertCommand();
            int affectedRecords = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (affectedRecords <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertOtherExpensesItems);
            }
        }
        public async Task Insert(IEnumerable<OtherExpensesItemsInsertDto> inputDto)
        {
            string command = GetInsertCommand();
            int affectedRecords = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (affectedRecords != (inputDto?.Count() ?? 0))
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertOtherExpensesItems);
            }
        }
        public async Task Remove(OtherExpensesItemsRemoveDto inputDto)
        {
            string command = GetRemoveCommand();
            int affectedRecords = await _connection.ExecuteAsync(command, inputDto, _transaction);
            if (affectedRecords <= 0)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidDeleteOtherExpensesItems);
            }
        }

        private string GetInsertCommand()
        {
            return $@"";
        }
        private string GetRemoveCommand()
        {
            return $@"";
        }

    }
}
