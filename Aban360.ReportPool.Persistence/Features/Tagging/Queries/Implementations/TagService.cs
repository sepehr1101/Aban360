using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Domain.Features.Tagging.Commands;
using Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.Tagging.Queries.Implementations
{
    internal sealed class TagService : AbstractBaseConnection, ITagService
    {
        public TagService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<TagDto>> GetAll()
        {
            var sql = @"SELECT 
                        	t.Id,
                        	t.Title,
                        	t.TagGroupId, 
                        	t.TagGroupTitle, 
                        	t.StringCode ,
                        	tg.MainTagGroupId ,
                        	mtg.Title MainTagGroupTitle
                        FROM CustomerWarehouse.dbo.Tags t
                        Join CustomerWarehouse.dbo.TagGroups tg
                        	ON t.TagGroupId=tg.Id
                        Join CustomerWarehouse.dbo.MainTagGroup mtg
                        	ON tg.MainTagGroupId=mtg.Id
                        WHERE
                        	t.DeleteDateTime IS NULL ";
            return await _sqlReportConnection.QueryAsync<TagDto>(sql);
        }
        public async Task<TagDto?> GetById(int id)
        {
            var sql = @"Select
                        	t.Id,
                        	t.Title,
                        	t.TagGroupId, 
                        	t.TagGroupTitle, 
                        	t.StringCode ,
                        	tg.MainTagGroupId ,
                        	mtg.Title MainTagGroupTitle
                        FROM CustomerWarehouse.dbo.Tags t
                        Join CustomerWarehouse.dbo.TagGroups tg
                        	ON t.TagGroupId=tg.Id
                        Join CustomerWarehouse.dbo.MainTagGroup mtg
                        	ON tg.MainTagGroupId=mtg.Id
                        WHERE
                        	t.DeleteDateTime IS NULL AND
                        	t.Id = @Id";
            return await _sqlReportConnection.QueryFirstOrDefaultAsync<TagDto>(sql, new { Id = id });
        }
    }
}
