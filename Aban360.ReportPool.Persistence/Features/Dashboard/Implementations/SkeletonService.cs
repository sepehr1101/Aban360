using Aban360.Common.Db.Dapper;
using Microsoft.Extensions.Configuration;
using Dapper;
using Aban360.ReportPool.Domain.Features.Dashboard.Entities;
using Aban360.ReportPool.Persistence.Features.Dashboard.Contracts;
using Aban360.ReportPool.Domain.Features.Dashboard.Dtos;

namespace Aban360.ReportPool.Persistence.Features.Dashboard.Implementations
{
    internal sealed class SkeletonService : AbstractBaseConnection, ISkeletonService
    {
        public SkeletonService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<IEnumerable<SkeletonDefinitionDto>> GetDefinitions()
        {
            string GetSql() => @"
                SELECT 
                    Id,
                    Name,
                    RoleId,
                    RoleTitle
                FROM [ReportPool].Skeleton
                WHERE DeleteDateTime IS NULL
                ORDER BY Id DESC;";

            return await _sqlConnection.QueryAsync<SkeletonDefinitionDto>(GetSql());
        }

        public async Task<Skeleton?> GetById(int id)
        {
            string GetSql() => @"
                SELECT *
                FROM [ReportPool].Skeleton
                WHERE Id = @Id AND DeleteDateTime IS NULL;";

            return await _sqlConnection.QueryFirstOrDefaultAsync<Skeleton>(GetSql(), new { Id = id });
        }
        public async Task<Skeleton?> GetByRole(string role)
        {
            string GetSql() => @"
                SELECT *
                FROM [ReportPool].Skeleton
                WHERE RoleTitle = @RoleName AND DeleteDateTime IS NULL;";

            return await _sqlConnection.QueryFirstOrDefaultAsync<Skeleton>(GetSql(), new { RoleName = role });
        }
        
        public async Task<IEnumerable<Skeleton>?> GetAllByRole(string role)
        {
            string GetSql() => @"
                SELECT *
                FROM [ReportPool].Skeleton
                WHERE RoleTitle = @RoleName AND DeleteDateTime IS NULL;";

            return await _sqlConnection.QueryAsync<Skeleton>(GetSql(), new { RoleName = role });
        }

        public async Task<int> Create(Skeleton entity)
        {
            string GetSql() => @"
                INSERT INTO [ReportPool].Skeleton (Name, RoleId, RoleTitle, Content, CreateDateTime, CreatedBy)
                VALUES (@Name, @RoleId, @RoleTitle, @Content, @CreateDateTime, @CreatedBy);
                SELECT CAST(SCOPE_IDENTITY() AS int);";

            return await _sqlConnection.ExecuteScalarAsync<int>(GetSql(), entity);
        }

        public async Task<bool> Update(Skeleton entity)
        {
            string GetSql() => @"
                UPDATE [ReportPool].Skeleton
                SET 
                    Name = @Name,
                    RoleId = @RoleId,
                    RoleTitle = @RoleTitle,
                    Content = @Content
                WHERE 
                    Id = @Id AND 
                    DeleteDateTime IS NULL;";

            int affected = await _sqlConnection.ExecuteAsync(GetSql(), entity);
            return affected > 0;
        }

        public async Task<bool> Delete(int id, string deletedBy)
        {
            string GetSql() => @"
                UPDATE [ReportPool].Skeleton
                SET 
                    DeleteDateTime = GETDATE(),
                    DeletedBy = @DeletedBy
                WHERE 
                    Id = @Id AND 
                    DeleteDateTime IS NULL;";

            int affected = await _sqlConnection.ExecuteAsync(GetSql(), new { Id = id, DeletedBy = deletedBy });
            return affected > 0;
        }
    }
}
