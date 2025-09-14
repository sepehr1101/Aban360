using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.Tagging;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.Tagging
{
    public interface ITagGroupService
    {
        Task<int> Create(CreateTagGroupDto dto);
        Task<bool> Delete(int id);
        Task<IEnumerable<TagGroupDto>> GetAll();
        Task<TagGroupDto?> GetById(int id);
        Task<bool> Update(UpdateTagGroupDto dto);
    }

    internal sealed class TagGroupService : AbstractBaseConnection, ITagGroupService
    {
        public TagGroupService(IConfiguration configuration) :
            base(configuration)
        {
        }

        public async Task<int> Create(CreateTagGroupDto dto)
        {
            var sql = @"
                INSERT INTO TagGroups (Title, CreateDateTime)
                VALUES (@Title, GETDATE());
                SELECT CAST(SCOPE_IDENTITY() as int);";

            return await _sqlReportConnection.ExecuteScalarAsync<int>(sql, dto);
        }
        public async Task<IEnumerable<TagGroupDto>> GetAll()
        {
            var sql = "SELECT Id, Title, CreateDateTime, DeleteDateTime FROM TagGroups";
            return await _sqlReportConnection.QueryAsync<TagGroupDto>(sql);
        }

        public async Task<TagGroupDto?> GetById(int id)
        {
            var sql = "SELECT Id, Title, CreateDateTime, DeleteDateTime FROM TagGroups WHERE Id = @Id";
            return await _sqlReportConnection.QueryFirstOrDefaultAsync<TagGroupDto>(sql, new { Id = id });
        }

        public async Task<bool> Update(UpdateTagGroupDto dto)
        {
            var sql = @"
                UPDATE TagGroups
                SET Title = @Title
                WHERE Id = @Id";

            var rows = await _sqlReportConnection.ExecuteAsync(sql, dto);
            return rows > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var sql = @"
                UPDATE TagGroups
                SET DeleteDateTime = GETDATE()
                WHERE Id = @Id";

            var rows = await _sqlReportConnection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}
