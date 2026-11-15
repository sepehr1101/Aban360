using Dapper;
using Aban360.Common.Extensions;
using System.Data;

namespace Aban360.OldCalcPool.Persistence.Features.WaterReturn.Command.Implementations
{
    public sealed class ReturnBillMinutesCounterCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public ReturnBillMinutesCounterCommandService(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));
            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
            if (!ReferenceEquals(transaction.Connection, connection))
            {
                throw new ArgumentException("تراکنش باید متعلق به اتصال ثبت برگشت باشد.", nameof(transaction));
            }
        }

        public async Task<int> GetNext(CancellationToken cancellationToken = default)
        {
            const string query = @"UPDATE Atlas.dbo.ReturnBillMinutesCounter
                SET LastNumber = LastNumber + 1,
                    UpdatedAt = SYSUTCDATETIME()
                OUTPUT INSERTED.LastNumber
                WHERE Id = 1;";

            var command = new CommandDefinition(query, transaction: _transaction,
                cancellationToken: cancellationToken);
            int? number = await _connection.QuerySingleOrDefaultAsync<int?>(command);
            if (!number.HasValue || number.Value <= 0)
            {
                throw new InvalidOperationException("شمارنده شماره جلسه در پایگاه‌داده Atlas مقداردهی نشده است.");
            }

            return number.Value;
        }
    }
}
