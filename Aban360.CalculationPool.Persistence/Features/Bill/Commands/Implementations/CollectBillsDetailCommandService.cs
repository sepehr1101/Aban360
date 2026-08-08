using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.Exceptions;
using Aban360.Common.Extensions;
using Aban360.Common.Literals;
using Dapper;
using System.Data;

namespace Aban360.CalculationPool.Persistence.Features.Bill.Commands.Implementations
{
    public sealed class CollectBillsDetailCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public CollectBillsDetailCommandService(
            IDbConnection connection,
            IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task<int> Insert(CollectBillsDetailInsertDto input)
        {
            string command = GetInsertCommand();
            int newId = await _connection.QueryFirstOrDefaultAsync<int>(command, input, _transaction);
            if (newId <= 0)
            {
                throw new ReadingException(ExceptionLiterals.InvalidInsertCollectBillDetail);
            }

            return newId;
        }
        public async Task Update(CollectBillsDetailUpdateDto input)
        {
            string query = GetUpdateCommand();
            int rowEffected = await _connection.ExecuteAsync(query, input, _transaction);
            if (rowEffected <= 0)
            {
                throw new ReadingException(ExceptionLiterals.InvalidUpdateCollectBillDetail);
            }
        }
        private string GetInsertCommand()
        {
            return @"Insert Into  [Atlas].dbo.CollectBillsDetail ( GroupingId, StepId, InsertDateTime, FinishDateTime, Description )
                    Values( @GroupingId, @StepId, @InsertDateTime, @FinishDateTime, @Description );

                    Select SCOPE_IDENTITY();";
        }
        private string GetUpdateCommand()
        {
            return @"Update Atlas.dbo.CollectBillsDetail
                    Set FinishDateTime = @FinishDateTime , Description = Description+'-'+@Description
                    Where Id = @Id";
        }
    }
}
