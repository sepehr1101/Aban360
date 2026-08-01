using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.Tagging;
using Aban360.ReportPool.Persistence.Features.Tagging.Queries.Contracts;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ReportPool.Persistence.Features.Tagging.Queries.Implementations
{
    internal sealed class TagQueryService : AbstractBaseConnection, ITagQueryService
    {
        public TagQueryService(IConfiguration configuration)
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
        public async Task<TagDto?> GetByStringCode(string stringCode)
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
                        	t.StringCode = @StringCode";
            return await _sqlReportConnection.QueryFirstOrDefaultAsync<TagDto>(sql, new { stringCode });
        }
        public async Task<IEnumerable<TagsStringCodeValidateDto>> ValidateStringCodes(IEnumerable<string> stringCodes, IDbConnection connection, IDbTransaction transaction)
        {
            DataTable table = new DataTable();
            table.Columns.Add("StringCode", typeof(string));
            table.Columns.Add("IsValid", typeof(bool));
            foreach (string code in stringCodes)
            {
                table.Rows.Add(code, false);
            }

            await connection.ExecuteAsync(GetTemplateTagTableCreateCommand(), null, transaction);
            using (var bulk = new SqlBulkCopy((SqlConnection)connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction))
            {
                bulk.DestinationTableName = "#TagsStringCodeTemplate";
                bulk.BatchSize = 10000;
                await bulk.WriteToServerAsync(table);
            };
            await connection.ExecuteAsync(GetUpdateTemplateTagTableCommand(), null, transaction);
            IEnumerable<TagsStringCodeValidateDto> result = await connection.QueryAsync<TagsStringCodeValidateDto>(GetTemplateTagTableQuery(), null, transaction);
            return result;
        }
        private string GetTemplateTagTableCreateCommand()
        {
            return @"Create Table #TagsStringCodeTemplate
                    (
                        StringCode varchar(15) NOT NULL,
                        IsValid bit NOT NULL
                    )";
        }
        private string GetUpdateTemplateTagTableCommand()
        {
            return @"Update tmp
                    Set tmp.IsValid = 1
                    From #TagsStringCodeTemplate tmp
                    Join CustomerWarehouse.dbo.Tags t
                        ON tmp.StringCode Collate Persian_100_CI_AI = t.StringCode Collate Persian_100_CI_AI
                    Where t.DeleteDateTime IS NULL";
        }
        private string GetTemplateTagTableQuery()
        {
            return @"Select * 
                    From #TagsStringCodeTemplate";
        }
    }
}
