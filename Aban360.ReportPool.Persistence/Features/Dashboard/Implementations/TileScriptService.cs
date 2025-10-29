using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.Dashboard.Dtos;
using Aban360.ReportPool.Domain.Features.Dashboard.Entities;
using Aban360.ReportPool.Persistence.Features.Dashboard.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.Dashboard.Implementations
{
    internal sealed class TileScriptService : AbstractBaseConnection, ITileScriptService
    {
        public TileScriptService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<int> Create(TileScript entity)
        {
            string Sql()
            {
                return @"
                    INSERT INTO [ReportPool].TileScript 
                        (ReportCode, Description, Content, CreateDateTime, CreatedBy)
                    VALUES 
                        (@ReportCode, @Description, @Content, @CreateDateTime, @CreatedBy);
                    SELECT CAST(SCOPE_IDENTITY() as int);";
            }

            return await _sqlConnection.ExecuteScalarAsync<int>(Sql(), entity);
        }

        public async Task<TileScript?> GetById(int id)
        {
            string Sql()
            {
                return @"
                    SELECT * 
                    FROM [ReportPool].TileScript 
                    WHERE Id = @Id AND DeleteDateTime IS NULL;";
            }

            return await _sqlConnection.QueryFirstOrDefaultAsync<TileScript>(Sql(), new { Id = id });
        }

        public async Task<IEnumerable<TileScriptReportDto>> GetContent(string content)
        {
            IEnumerable<TileScriptReportDto>? report=await _sqlReportConnection.QueryAsync<TileScriptReportDto>(content,null);
            
            return report;
        }
        public async Task<IEnumerable<TileScript>> GetAll()
        {
            string Sql()
            {
                return @"
                    SELECT * 
                    FROM [ReportPool].TileScript 
                    WHERE DeleteDateTime IS NULL 
                    ORDER BY CreateDateTime DESC;";
            }

            return await _sqlConnection.QueryAsync<TileScript>(Sql());
        }

        public async Task<bool> Update(TileScriptDto entity)
        {
            string Sql()
            {
                return @"
                    UPDATE [ReportPool].TileScript
                    SET 
                        ReportCode = @ReportCode,
                        Description = @Description,
                        Content = @Content
                    WHERE Id = @Id AND DeleteDateTime IS NULL;";
            }

            var rows = await _sqlConnection.ExecuteAsync(Sql(), entity);
            return rows > 0;
        }

        public async Task<bool> Delete(int id, string deletedBy)
        {
            string Sql()
            {
                return @"
                    UPDATE [ReportPool].TileScript
                    SET 
                        DeleteDateTime = @DeleteDateTime,
                        DeletedBy = @DeletedBy
                    WHERE Id = @Id AND DeleteDateTime IS NULL;";
            }

            var rows = await _sqlConnection.ExecuteAsync(Sql(), new
            {
                Id = id,
                DeleteDateTime = DateTime.Now,
                DeletedBy = deletedBy
            });

            return rows > 0;
        }
    }
}
