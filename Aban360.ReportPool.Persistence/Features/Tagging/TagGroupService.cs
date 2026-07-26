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
        Task<TagGroupDto?> GetByStringCode(string input);
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
                INSERT INTO TagGroups (Title, StringCode, MainTagGroupId, CreateDateTime)
                VALUES (@Title, @StringCode, @MainTagGroupId, GETDATE());
                SELECT CAST(SCOPE_IDENTITY() as int);";

            return await _sqlReportConnection.ExecuteScalarAsync<int>(sql, dto);
        }
        public async Task<IEnumerable<TagGroupDto>> GetAll()
        {
            var sql = @"SELECT t.Id, t.Title, t.StringCode, t.MainTagGroupId,  m.Title MainTagGroupTitle, t.CreateDateTime, t.DeleteDateTime 
                        FROM CustomerWarehouse.dbo.TagGroups t
                        Join CustomerWarehouse.dbo.MainTagGroup m
                            ON t.MainTagGroupId = m.Id";
            return await _sqlReportConnection.QueryAsync<TagGroupDto>(sql);
        }

        public async Task<TagGroupDto?> GetById(int id)
        {
            var sql = @"SELECT t.Id, t.Title, t.StringCode, t.MainTagGroupId,  m.Title MainTagGroupTitle, t.CreateDateTime, t.DeleteDateTime 
                        FROM CustomerWarehouse.dbo.TagGroups t
                        Join CustomerWarehouse.dbo.MainTagGroup m
                            ON t.MainTagGroupId = m.Id
                        WHERE t.Id = @Id";
            return await _sqlReportConnection.QueryFirstOrDefaultAsync<TagGroupDto>(sql, new { Id = id });
        }
        public async Task<TagGroupDto?> GetByStringCode(string input)
        {
            var sql = "SELECT Id, Title, StringCode, MainTagGroupId, CreateDateTime, DeleteDateTime FROM TagGroups WHERE StringCode = @input";
            return await _sqlReportConnection.QueryFirstOrDefaultAsync<TagGroupDto>(sql, new { input });
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
