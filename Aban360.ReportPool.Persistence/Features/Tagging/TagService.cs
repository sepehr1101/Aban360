using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.Tagging;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.Tagging
{
    public interface ITagService
    {
        Task<int> Create(CreateTagDto dto);
        Task<bool> Delete(int id);
        Task<IEnumerable<TagDto>> GetAll();
        Task<TagDto?> GetById(int id);
        Task<bool> Update(UpdateTagDto dto);
    }

    internal sealed class TagService : AbstractBaseConnection, ITagService
    {
        public TagService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<int> Create(CreateTagDto dto)
        {
            var sql = @"
                INSERT INTO Tags (Title, TagGroupId, TagGroupTitle)
                SELECT @Title, @TagGroupId, tg.Title
                FROM TagGroups tg
                WHERE tg.Id = @TagGroupId;
                SELECT CAST(SCOPE_IDENTITY() as int);";

            return await _sqlReportConnection.ExecuteScalarAsync<int>(sql, dto);
        }

        public async Task<IEnumerable<TagDto>> GetAll()
        {
            var sql = "SELECT Id, Title, TagGroupId, TagGroupTitle FROM Tags";
            return await _sqlReportConnection.QueryAsync<TagDto>(sql);
        }

        public async Task<TagDto?> GetById(int id)
        {
            var sql = "SELECT Id, Title, TagGroupId, TagGroupTitle FROM Tags WHERE Id = @Id";
            return await _sqlReportConnection.QueryFirstOrDefaultAsync<TagDto>(sql, new { Id = id });
        }

        public async Task<bool> Update(UpdateTagDto dto)
        {
            var sql = @"
                UPDATE Tags
                SET Title = @Title,
                    TagGroupId = @TagGroupId,
                    TagGroupTitle = (SELECT Title FROM TagGroups WHERE Id = @TagGroupId)
                WHERE Id = @Id";

            var rows = await _sqlReportConnection.ExecuteAsync(sql, dto);
            return rows > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var sql = "DELETE FROM Tags WHERE Id = @Id";
            var rows = await _sqlReportConnection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}
