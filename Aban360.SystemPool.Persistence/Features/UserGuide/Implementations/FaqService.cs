using Aban360.Common.Db.Dapper;
using Aban360.SystemPool.Domain.Features.UserGuide.Entities;
using Microsoft.Extensions.Configuration;
using Dapper;
using Aban360.SystemPool.Domain.Features.UserGuide.Dtos;
using Aban360.SystemPool.Persistence.Features.UserGuide.Contracts;

namespace Aban360.SystemPool.Persistence.Features.UserGuide.Implementations
{
    internal sealed class FaqService : AbstractBaseConnection, IFaqService
    {
        public FaqService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<int> Create(Faq faq)
        {
            string sql = GetSql();
            return await _sqlConnection.ExecuteScalarAsync<int>(sql, faq);

            string GetSql()
            {
                const string sql = @"
                    INSERT INTO [UserGuidePool].Faq (Header, Icon, Content, CreateDateTime, CreatedBy, DeleteDateTime, DeletedBy)
                    VALUES (@Header, @Icon, @Content, @CreateDateTime, @CreatedBy, @DeleteDateTime, @DeletedBy);
                    SELECT CAST(SCOPE_IDENTITY() as int);";
                return sql;
            }
        }

        public async Task<Faq?> Get(int id)
        {
            string sql = GetSql();
            return await _sqlConnection.QueryFirstOrDefaultAsync<Faq>(sql, new { Id = id });

            string GetSql()
            {
                const string sql = "SELECT * FROM [UserGuidePool].Faq WHERE Id = @Id;";
                return sql;
            }
        }

        public async Task<IEnumerable<FaqGetAllDto>> Get()
        {
            string sql = GetSql();
            return await _sqlConnection.QueryAsync<FaqGetAllDto>(sql);

            string GetSql()
            {
                const string sql = @"
                    SELECT Id, Header, Icon 
                    FROM [UserGuidePool].Faq 
                    WHERE DeleteDateTime IS NULL 
                    ORDER BY CreateDateTime DESC;";
                return sql;
            }
        }

        public async Task<int> Update(FaqDto faq)
        {
            string sql = GetSql();

            return await _sqlConnection.ExecuteAsync(sql, faq);
            string GetSql()
            {
                string sql = @"
                    UPDATE [UserGuidePool].Faq
                    SET Header = @Header,
                        Icon = @Icon,
                        Content = @Content
                    WHERE Id = @Id;";
                return sql;
            }
        }

        public async Task<int> Delete(int id, string deletedBy)
        {
            string sql = GetSql();
            return await _sqlConnection.ExecuteAsync(sql, new { Id = id, DeletedBy = deletedBy });
            string GetSql()
            {
                const string sql = @"
                    UPDATE [UserGuidePool].Faq
                    SET DeleteDateTime = GETDATE(),
                        DeletedBy = @DeletedBy
                    WHERE Id = @Id;";
                return sql;
            }
        }
    }
}
