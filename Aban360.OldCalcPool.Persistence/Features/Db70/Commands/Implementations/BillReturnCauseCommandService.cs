using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Dapper;
using System.Data;

namespace Aban360.OldCalcPool.Persistence.Features.Db70.Commands.Implementations
{
    public sealed class BillReturnCauseCommandService //: IBillReturnCauseCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public BillReturnCauseCommandService(
            IDbConnection connection,
            IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }
        public async Task Create(BillReturnCauseCreateDto input)
        {
            string query = GetCreateQuery();
            await _connection.ExecuteAsync(query, input, _transaction);
        }
        public async Task Update(BillReturnCauseUpdateDto input)
        {
            string query = GetUpdateQuery();
            await _connection.ExecuteAsync(query, input, _transaction);
        }
        public async Task Delete(BillReturnCauseDeleteDto input)
        {
            string query = GetDeleteQuery();
            await _connection.ExecuteAsync(query, input, _transaction);
        }


        private string GetCreateQuery()
        {
            return @"use [Db70]
                    Insert Into [Db70].dbo.BillReturnCause(Code,Title,IsInList,IsLastMeterValid,IsPartial,RegisterDateTime,RegisterByUserId)
                    Values(@Code,@Title,@IsInList,@IsLastMeterValid,@IsPartial,@RegisterDateTime,@RegisterByUserId)";
        }
        private string GetUpdateQuery()
        {
            return @"use [Db70]
                    Update [Db70].dbo.BillReturnCause
                    Set Code = @Code , Title = @Title , IsInList = @IsInList , IsLastMeterValid = @IsLastMeterValid , IsPartial = @IsPartial
                    Where 
                        RemoveDateTime IS NULL AND 
                        Id=@Id";
        }
        private string GetDeleteQuery()
        {
            return @"use [Db70]
                    Update [Db70].dbo.BillReturnCause
                    Set RemoveDateTime=@RemoveDateTime , RemoveByUserId=@RemoveByUserId
                    Where Id=@Id";
        }

    }
}
