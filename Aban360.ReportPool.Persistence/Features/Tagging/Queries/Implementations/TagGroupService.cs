using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Domain.Features.Tagging.Commands;
using Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.Tagging.Queries.Implementations
{
    internal sealed class TagGroupService : AbstractBaseConnection, ITagGroupService
    {
        public TagGroupService(IConfiguration configuration) :
            base(configuration)
        {
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
        public async Task<IEnumerable<TagGroupDto>> GetByMainTagGroupId(int id)
        {
            var sql = @"SELECT t.Id, t.Title, t.StringCode, t.MainTagGroupId,  m.Title MainTagGroupTitle, t.CreateDateTime, t.DeleteDateTime 
                        FROM CustomerWarehouse.dbo.TagGroups t
                        Join CustomerWarehouse.dbo.MainTagGroup m
                            ON t.MainTagGroupId = m.Id
                        WHERE m.Id = @Id";
            return await _sqlReportConnection.QueryAsync<TagGroupDto>(sql, new { Id = id });
        }
        public async Task<TagGroupDto?> GetByStringCode(string input)
        {
            var sql = "SELECT Id, Title, StringCode, MainTagGroupId, CreateDateTime, DeleteDateTime FROM TagGroups WHERE StringCode = @input";
            return await _sqlReportConnection.QueryFirstOrDefaultAsync<TagGroupDto>(sql, new { input });
        }
       
    }
}
