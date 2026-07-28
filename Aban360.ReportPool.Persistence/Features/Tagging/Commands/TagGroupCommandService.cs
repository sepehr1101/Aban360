using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.Tagging.Commands;
using Dapper;
using System.Data;

namespace Aban360.ReportPool.Persistence.Features.Tagging.Commands
{
    public class TagGroupCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public TagGroupCommandService(
            IDbConnection connection, 
            IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task<int> Create(CreateTagGroupDto dto)
        {
            var sql = @"
                INSERT INTO TagGroups (Title, StringCode, MainTagGroupId, CreateDateTime)
                VALUES (@Title, @StringCode, @MainTagGroupId, GETDATE());
                SELECT CAST(SCOPE_IDENTITY() as int);";

            return await _connection.ExecuteScalarAsync<int>(sql, dto, _transaction);
        }
        public async Task<bool> Update(UpdateTagGroupDto dto)
        {
            var sql = @"
                UPDATE TagGroups
                SET 
                    Title = @Title ,
                    StringCode = @StringCode,
                    MainTagGroupId = @MainTagGroupId
                WHERE Id = @Id";

            var rows = await _connection.ExecuteAsync(sql, dto, _transaction);
            return rows > 0;
        }
        public async Task<bool> Delete(int id)
        {
            var sql = @"UPDATE CustomerWarehouse.dbo.TagGroups
                        SET DeleteDateTime = GETDATE()
                        WHERE Id = @Id";

            var rows = await _connection.ExecuteAsync(sql, new { Id = id }, _transaction);
            return rows > 0;
        }
        public async Task<bool> DeleteByMainGroupId(int id)
        {
            var sql = @"UPDATE CustomerWarehouse.dbo.TagGroups
                        SET DeleteDateTime = GETDATE()
                        WHERE MainTagGroupId  = @Id";

            var rows = await _connection.ExecuteAsync(sql, new { Id = id }, _transaction);
            return rows > 0;
        }
    }
}
