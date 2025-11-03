using Aban360.Common.Db.Dapper;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Commands;
using Aban360.OldCalcPool.Persistence.Features.Db70.Commands.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.OldCalcPool.Persistence.Features.Db70.Commands.Implementations
{
    internal sealed class VirtualCategoryCommandService : AbstractBaseConnection, IVirtualCategoryCommandService
    {
        public VirtualCategoryCommandService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task Create(VirtualCategoryCreateDto input)
        {
            string query = GetCreateQuery();
            await _sqlReportConnection.ExecuteAsync(query, input);
        }
        public async Task Update(VirtualCategoryUpdateDto input)
        {
            string query = GetUpdateQuery();
            await _sqlReportConnection.ExecuteAsync(query, input);
        }
        public async Task Delete(VirtualCategorySearchInputDto input)
        {
            string query = GetDeleteQuery();
            await _sqlReportConnection.ExecuteAsync(query, input);
        }


        private string GetCreateQuery()
        {
            return @"use [Db70]
                    Insert Into [Db70].dbo.VirtualCategory(Code,Title,Multiplier)
                    Values(@Code,@Title,@Multiplier)";
        }
        private string GetUpdateQuery()
        {
            return @"use [Db70]
                    Update [Db70].dbo.VirtualCategory
                    Set Code=@Code, Title=@Title, Multiplier=@Multiplier
                    Where Id=@Id";
        }
        private string GetDeleteQuery()
        {
            return @"use [Db70]
                    Delete From [Db70].dbo.VirtualCategory
                    Where Id=@Id";
        }

    }
}
