using Aban360.ClaimPool.Domain.Features.Tracking.Dto;
using Aban360.ClaimPool.Persistence.Constants.Literals;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Dapper;
using System.Data;

namespace Aban360.ClaimPool.Persistence.Features.Tracking.Commands.Implementations
{
    public class QueueCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public QueueCommandService(
            IDbConnection connection,
            IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task<Guid> Insert(QueueInsertDto inputDto)
        {
            string command = GetInsertCommand();
            Guid? rowId = await _connection.QueryFirstOrDefaultAsync<Guid>(command, inputDto, _transaction);
            if (rowId is null)
            {
                throw new InvalidTrackingException(ExceptionLiterals.InvalidInsertQueue);
            }
            return rowId.Value;
        }
        private string GetInsertCommand()
        {
            return $@"


                    SELECT CAST(SCOPE_IDENTITY() AS INT)";
        }
    }
}
