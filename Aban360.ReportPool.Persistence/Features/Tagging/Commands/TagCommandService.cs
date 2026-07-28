using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.Tagging.Commands;
using Dapper;
using System.Data;

namespace Aban360.ReportPool.Persistence.Features.Tagging.Commands
{
    public class TagCommandService
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public TagCommandService(
            IDbConnection connection,
            IDbTransaction transaction)
        {
            _connection = connection;
            _connection.NotNull(nameof(connection));

            _transaction = transaction;
            _transaction.NotNull(nameof(transaction));
        }

        public async Task<int> Create(CreateTagDto dto)
        {
            var sql = @"
                INSERT INTO Tags (Title, TagGroupId, TagGroupTitle, StringCode)
                SELECT @Title, @TagGroupId, tg.Title, @StringCode
                FROM TagGroups tg
                WHERE tg.Id = @TagGroupId;
                SELECT CAST(SCOPE_IDENTITY() as int);";

            return await _connection.ExecuteScalarAsync<int>(sql, dto, _transaction);
        }
        public async Task<bool> Update(UpdateTagDto dto)
        {
            var sql = @"
                UPDATE Tags
                SET Title = @Title,
                    TagGroupId = @TagGroupId,
                    TagGroupTitle = (SELECT Title FROM TagGroups WHERE Id = @TagGroupId),
                    StringCode= @StringCode
                WHERE Id = @Id";

            var rows = await _connection.ExecuteAsync(sql, dto, _transaction);
            return rows > 0;
        }
        public async Task<bool> UpdateTagGroupTitle(int id, string title)
        {
            var sql = @"UPDATE CustomerWarehouse.dbo.Tags
                        SET TagGroupTitle = @title
                        WHERE TagGroupId = @Id";

            var rows = await _connection.ExecuteAsync(sql, new { title, id }, _transaction);
            return rows > 0;
        }
        public async Task<bool> Delete(int id)
        {
            var sql = @"Update CustomerWarehouse.dbo.Tags
                        Set DeleteDateTime=GETDATE()
                        Where Id = @Id";
            var rows = await _connection.ExecuteAsync(sql, new { Id = id }, _transaction);
            return rows > 0;
        }
        public async Task<bool> DeleteByTagGroupId(int id)
        {
            var sql = @"Update CustomerWarehouse.dbo.Tags
                        Set DeleteDateTime=GETDATE()
                        Where TagGroupId = @Id";
            var rows = await _connection.ExecuteAsync(sql, new { Id = id }, _transaction);
            return rows > 0;
        }
        public async Task<bool> DeleteByTagGroupId(IEnumerable<int> id)
        {
            var sql = @"Update CustomerWarehouse.dbo.Tags
                        Set DeleteDateTime=GETDATE()
                        Where TagGroupId In @Id";
            var rows = await _connection.ExecuteAsync(sql, new { Id = id }, _transaction);
            return rows > 0;
        }
    }
}
